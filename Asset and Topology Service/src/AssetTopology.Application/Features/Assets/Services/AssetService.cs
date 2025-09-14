using AssetTopology.Application.Interfaces;
using AssetTopology.Domain.Entities;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System;
using AssetTopology.Application.Models;
using AssetTopology.Application.Exceptions;
using System.Linq;

namespace AssetTopology.Application.Features.Assets.Services
{
    /// <summary>
    /// Implements the application logic for managing assets, including the cache-aside pattern for hierarchy retrieval.
    /// </summary>
    public class AssetService : IAssetService
    {
        private readonly IAssetRepository _assetRepository;
        private readonly IAssetCacheRepository _assetCacheRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<AssetService> _logger;

        public AssetService(
            IAssetRepository assetRepository,
            IAssetCacheRepository assetCacheRepository,
            IUnitOfWork unitOfWork,
            ILogger<AssetService> logger)
        {
            _assetRepository = assetRepository ?? throw new ArgumentNullException(nameof(assetRepository));
            _assetCacheRepository = assetCacheRepository ?? throw new ArgumentNullException(nameof(assetCacheRepository));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<AssetNode?> GetAssetHierarchyAsync(Guid tenantId, CancellationToken cancellationToken)
        {
            try
            {
                var cachedHierarchy = await _assetCacheRepository.GetHierarchyAsync(tenantId, cancellationToken);
                if (cachedHierarchy != null)
                {
                    _logger.LogInformation("Cache hit for asset hierarchy for tenant {TenantId}", tenantId);
                    return cachedHierarchy;
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to retrieve asset hierarchy from cache for tenant {TenantId}. Falling back to database.", tenantId);
            }

            _logger.LogInformation("Cache miss for asset hierarchy for tenant {TenantId}. Fetching from database.", tenantId);
            var assets = await _assetRepository.GetHierarchyByTenantIdAsync(tenantId, cancellationToken);

            var hierarchyRoot = BuildHierarchy(assets);

            if (hierarchyRoot != null)
            {
                try
                {
                    await _assetCacheRepository.SetHierarchyAsync(tenantId, hierarchyRoot, cancellationToken);
                    _logger.LogInformation("Asset hierarchy for tenant {TenantId} stored in cache.", tenantId);
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Failed to store asset hierarchy in cache for tenant {TenantId}.", tenantId);
                }
            }

            return hierarchyRoot;
        }

        public async Task<Asset> CreateAssetAsync(Guid tenantId, string name, Guid? parentAssetId, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ValidationException("Asset name cannot be empty.");
            }

            if (await _assetRepository.ExistsByNameAndParentAsync(tenantId, name, parentAssetId, cancellationToken))
            {
                throw new DuplicateAssetNameException($"An asset with the name '{name}' already exists under the specified parent.");
            }

            var asset = new Asset
            {
                TenantId = tenantId,
                Name = name,
                ParentAssetId = parentAssetId
            };

            _assetRepository.Add(asset);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            await _assetCacheRepository.InvalidateHierarchyAsync(tenantId, cancellationToken);
            _logger.LogInformation("Created new asset {AssetName} ({AssetId}) for tenant {TenantId}. Cache invalidated.", asset.Name, asset.Id, tenantId);

            return asset;
        }

        public async Task UpdateAssetAsync(Guid tenantId, Guid assetId, string newName, CancellationToken cancellationToken)
        {
            var asset = await _assetRepository.GetByIdAsync(assetId, cancellationToken);

            if (asset == null || asset.TenantId != tenantId)
            {
                throw new AssetNotFoundException($"Asset with ID {assetId} not found for this tenant.");
            }
             
            if (string.IsNullOrWhiteSpace(newName))
            {
                throw new ValidationException("Asset name cannot be empty.");
            }

            if (asset.Name != newName && await _assetRepository.ExistsByNameAndParentAsync(tenantId, newName, asset.ParentAssetId, cancellationToken))
            {
                 throw new DuplicateAssetNameException($"An asset with the name '{newName}' already exists under the specified parent.");
            }

            var oldName = asset.Name;
            asset.Name = newName;
            _assetRepository.Update(asset);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            await _assetCacheRepository.InvalidateHierarchyAsync(tenantId, cancellationToken);
            _logger.LogInformation("Updated asset name from '{OldName}' to '{NewName}' ({AssetId}) for tenant {TenantId}. Cache invalidated.", oldName, newName, assetId, tenantId);
        }

        public async Task MoveAssetAsync(Guid tenantId, Guid assetId, Guid? newParentAssetId, CancellationToken cancellationToken)
        {
            var assetToMove = await _assetRepository.GetByIdAsync(assetId, cancellationToken);
            if (assetToMove == null || assetToMove.TenantId != tenantId)
            {
                throw new AssetNotFoundException($"Asset with ID {assetId} not found for this tenant.");
            }

            if (newParentAssetId.HasValue)
            {
                var newParentAsset = await _assetRepository.GetByIdAsync(newParentAssetId.Value, cancellationToken);
                if (newParentAsset == null || newParentAsset.TenantId != tenantId)
                {
                    throw new AssetNotFoundException($"Target parent asset with ID {newParentAssetId} not found for this tenant.");
                }

                // Circular dependency check
                var allAssets = await _assetRepository.GetHierarchyByTenantIdAsync(tenantId, cancellationToken);
                if (IsDescendant(allAssets, assetId, newParentAssetId.Value))
                {
                    throw new CircularDependencyException("Cannot move an asset under one of its own descendants.");
                }
            }
            
            if (await _assetRepository.ExistsByNameAndParentAsync(tenantId, assetToMove.Name, newParentAssetId, cancellationToken))
            {
                throw new DuplicateAssetNameException($"An asset with the name '{assetToMove.Name}' already exists under the specified target parent.");
            }

            assetToMove.ParentAssetId = newParentAssetId;
            _assetRepository.Update(assetToMove);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            await _assetCacheRepository.InvalidateHierarchyAsync(tenantId, cancellationToken);
            _logger.LogInformation("Moved asset {AssetName} ({AssetId}) to new parent {NewParentAssetId} for tenant {TenantId}. Cache invalidated.", assetToMove.Name, assetId, newParentAssetId, tenantId);
        }

        public async Task DeleteAssetAsync(Guid tenantId, Guid assetId, CancellationToken cancellationToken)
        {
            var assetToDelete = await _assetRepository.GetByIdAsync(assetId, cancellationToken);
            if (assetToDelete == null || assetToDelete.TenantId != tenantId)
            {
                throw new AssetNotFoundException($"Asset with ID {assetId} not found for this tenant.");
            }

            var allAssets = await _assetRepository.GetHierarchyByTenantIdAsync(tenantId, cancellationToken);
            var assetsToRemove = GetDescendantsAndSelf(allAssets, assetId);
            
            _assetRepository.RemoveRange(assetsToRemove);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            
            await _assetCacheRepository.InvalidateHierarchyAsync(tenantId, cancellationToken);
            _logger.LogInformation("Deleted asset {AssetName} ({AssetId}) and its {DescendantCount} descendants for tenant {TenantId}. Cache invalidated.", assetToDelete.Name, assetId, assetsToRemove.Count - 1, tenantId);
        }

        private AssetNode? BuildHierarchy(IEnumerable<Asset> allAssets)
        {
            var assetsList = allAssets.ToList();
            if (!assetsList.Any())
            {
                return null;
            }

            var assetMap = assetsList.ToDictionary(
                a => a.Id,
                a => new AssetNode { Id = a.Id, Name = a.Name, Children = new List<AssetNode>() });

            var rootAssets = new List<AssetNode>();

            foreach (var asset in assetsList)
            {
                if (asset.ParentAssetId.HasValue && assetMap.TryGetValue(asset.ParentAssetId.Value, out var parentNode))
                {
                    parentNode.Children.Add(assetMap[asset.Id]);
                }
                else
                {
                    rootAssets.Add(assetMap[asset.Id]);
                }
            }

            // In a well-formed hierarchy for a site, there should be one root. 
            // We'll wrap multiple roots under a synthetic root if needed for consistency.
            if (rootAssets.Count == 1)
            {
                return rootAssets.First();
            }
            
            return new AssetNode
            {
                Id = Guid.Empty, // Synthetic root
                Name = "Root",
                Children = rootAssets
            };
        }
        
        private bool IsDescendant(IEnumerable<Asset> allAssets, Guid potentialParentId, Guid childId)
        {
            var descendants = GetDescendantsAndSelf(allAssets, potentialParentId);
            return descendants.Any(d => d.Id == childId);
        }

        private List<Asset> GetDescendantsAndSelf(IEnumerable<Asset> allAssets, Guid parentId)
        {
            var assetList = allAssets.ToList();
            var result = new List<Asset>();
            var queue = new Queue<Guid>();
            
            var initialAsset = assetList.FirstOrDefault(a => a.Id == parentId);
            if (initialAsset == null) return result;

            result.Add(initialAsset);
            queue.Enqueue(parentId);

            var childrenLookup = assetList
                .Where(a => a.ParentAssetId.HasValue)
                .ToLookup(a => a.ParentAssetId.Value);

            while (queue.Count > 0)
            {
                var currentParentId = queue.Dequeue();
                if (childrenLookup.Contains(currentParentId))
                {
                    foreach (var child in childrenLookup[currentParentId])
                    {
                        result.Add(child);
                        queue.Enqueue(child.Id);
                    }
                }
            }
            return result;
        }
    }
}