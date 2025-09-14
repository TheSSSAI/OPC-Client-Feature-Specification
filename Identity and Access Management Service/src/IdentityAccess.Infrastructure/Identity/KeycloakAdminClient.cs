using Microsoft.Extensions.Options;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Services.IdentityAccess.Application.Common.Exceptions;
using System.Services.IdentityAccess.Application.Common.Interfaces;
using System.Services.IdentityAccess.Application.Common.Models;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace System.Services.IdentityAccess.Infrastructure.Identity;

public class KeycloakAdminClient : IKeycloakAdminClient
{
    private readonly HttpClient _httpClient;
    private readonly KeycloakSettings _settings;
    private static readonly SemaphoreSlim _tokenSemaphore = new(1, 1);
    private AccessTokenResponse? _accessToken;
    private DateTimeOffset _tokenExpiration;

    public KeycloakAdminClient(HttpClient httpClient, IOptions<KeycloakSettings> settings)
    {
        _httpClient = httpClient;
        _settings = settings.Value;
        _httpClient.BaseAddress = new Uri(_settings.AdminApiUrl);
    }

    public async Task<string> CreateUserAsync(KeycloakUserRepresentation user, CancellationToken cancellationToken)
    {
        await EnsureAuthenticatedAsync(cancellationToken);

        var request = new HttpRequestMessage(HttpMethod.Post, "users")
        {
            Content = JsonContent.Create(user, options: new JsonSerializerOptions
            {
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            })
        };

        var response = await _httpClient.SendAsync(request, cancellationToken);

        if (response.IsSuccessStatusCode)
        {
            // Keycloak returns the location of the new user in the Location header
            var locationHeader = response.Headers.Location;
            if (locationHeader != null)
            {
                return locationHeader.Segments.Last();
            }
            throw new KeycloakApiException("User created but could not retrieve user ID from Location header.");
        }
        
        if (response.StatusCode == System.Net.HttpStatusCode.Conflict)
        {
            throw new ConflictException("User with the same username or email already exists in Keycloak.");
        }

        var errorContent = await response.Content.ReadAsStringAsync(cancellationToken);
        throw new KeycloakApiException($"Failed to create user in Keycloak. Status: {response.StatusCode}. Response: {errorContent}");
    }

    private async Task EnsureAuthenticatedAsync(CancellationToken cancellationToken)
    {
        if (_accessToken != null && _tokenExpiration > DateTimeOffset.UtcNow)
        {
            return;
        }

        await _tokenSemaphore.WaitAsync(cancellationToken);
        try
        {
            // Double-check lock
            if (_accessToken != null && _tokenExpiration > DateTimeOffset.UtcNow)
            {
                return;
            }

            var requestContent = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                {"client_id", _settings.ClientId},
                {"client_secret", _settings.ClientSecret},
                {"grant_type", "client_credentials"}
            });

            var request = new HttpRequestMessage(HttpMethod.Post, _settings.TokenUrl) { Content = requestContent };
            var response = await _httpClient.SendAsync(request, cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync(cancellationToken);
                throw new KeycloakApiException($"Failed to authenticate with Keycloak. Status: {response.StatusCode}. Response: {errorContent}");
            }

            _accessToken = await response.Content.ReadFromJsonAsync<AccessTokenResponse>(cancellationToken);
            if (_accessToken == null)
            {
                throw new KeycloakApiException("Failed to deserialize access token from Keycloak.");
            }
            _tokenExpiration = DateTimeOffset.UtcNow.AddSeconds(_accessToken.ExpiresIn - 30); // 30-second buffer

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken.Token);
        }
        finally
        {
            _tokenSemaphore.Release();
        }
    }

    private class AccessTokenResponse
    {
        [JsonPropertyName("access_token")]
        public string Token { get; set; } = string.Empty;

        [JsonPropertyName("expires_in")]
        public int ExpiresIn { get; set; }
    }
}