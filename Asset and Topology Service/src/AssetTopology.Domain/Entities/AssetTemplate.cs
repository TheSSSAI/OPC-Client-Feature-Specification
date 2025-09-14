using System.Text.Json;
using System.Shared.Domain;

namespace System.Services.AssetTopology.Domain.Entities
{
    /// <summary>
    /// Defines a reusable pattern for creating new assets with a predefined structure and properties.
    /// This entity supports REQ-1-048 by allowing engineers to standardize common equipment types.
    /// </summary>
    public class AssetTemplate : BaseEntity
    {
        /// <summary>
        /// The identifier for the tenant that owns this template. Essential for data isolation.
        /// </summary>
        public Guid TenantId { get; private set; }

        /// <summary>
        /// The human-readable name of the template (e.g., "Standard Pump Assembly").
        /// This name must be unique within the tenant.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// A detailed description of the template's purpose and intended use.
        /// </summary>
        public string? Description { get; private set; }

        /// <summary>
        /// A JSON string defining the structure, child assets, and default tag mappings
        /// to be created when this template is instantiated.
        /// </summary>
        public string PropertiesJson { get; private set; }

        /// <summary>
        /// Private constructor for EF Core.
        /// </summary>
        private AssetTemplate() { }

        /// <summary>
        /// Private constructor for the factory method.
        /// </summary>
        private AssetTemplate(Guid tenantId, string name, string? description, string propertiesJson)
        {
            TenantId = tenantId;
            Name = name;
            Description = description;
            PropertiesJson = propertiesJson;
        }

        /// <summary>
        /// Factory method to create a new AssetTemplate, ensuring initial validation.
        /// </summary>
        /// <param name="tenantId">The ID of the tenant owning the template.</param>
        /// <param name="name">The name of the template.</param>
        /// <param name="description">An optional description.</param>
        /// <param name="propertiesJson">The JSON definition of the template's properties and structure.</param>
        /// <returns>A new, valid AssetTemplate instance.</returns>
        /// <exception cref="ArgumentException">Thrown if tenantId is empty, name is null/whitespace, or propertiesJson is invalid.</exception>
        public static AssetTemplate Create(Guid tenantId, string name, string? description, string propertiesJson)
        {
            if (tenantId == Guid.Empty)
            {
                throw new ArgumentException("Tenant ID cannot be empty.", nameof(tenantId));
            }
            
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Template name cannot be null or whitespace.", nameof(name));
            }

            if (string.IsNullOrWhiteSpace(propertiesJson) || !IsValidJson(propertiesJson))
            {
                throw new ArgumentException("Properties JSON cannot be null, empty, or invalid.", nameof(propertiesJson));
            }

            return new AssetTemplate(tenantId, name, description, propertiesJson);
        }

        /// <summary>
        /// Updates the details of the asset template.
        /// </summary>
        /// <param name="name">The new name for the template.</param>
        /// <param name="description">The new description for the template.</param>
        /// <param name="propertiesJson">The new JSON definition for the template.</param>
        /// <exception cref="ArgumentException">Thrown if name is null/whitespace or propertiesJson is invalid.</exception>
        public void Update(string name, string? description, string propertiesJson)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Template name cannot be null or whitespace.", nameof(name));
            }

            if (string.IsNullOrWhiteSpace(propertiesJson) || !IsValidJson(propertiesJson))
            {
                throw new ArgumentException("Properties JSON cannot be null, empty, or invalid.", nameof(propertiesJson));
            }

            Name = name;
            Description = description;
            PropertiesJson = propertiesJson;
        }

        /// <summary>
        /// Validates if the given string is a valid JSON object or array.
        /// </summary>
        /// <param name="json">The string to validate.</param>
        /// <returns>True if the string is valid JSON, otherwise false.</returns>
        private static bool IsValidJson(string json)
        {
            try
            {
                JsonDocument.Parse(json);
                return true;
            }
            catch (JsonException)
            {
                return false;
            }
        }
    }
}