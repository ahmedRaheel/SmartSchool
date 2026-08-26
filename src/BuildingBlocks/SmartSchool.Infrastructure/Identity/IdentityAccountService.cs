using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Serilog;
using SmartSchool.Application.Identity;

namespace SmartSchool.Infrastructure.Identity;

public sealed class IdentityAccountService : IIdentityAccountService
{
    private readonly HttpClient _httpClient;
    private readonly IdentityServiceOptions _options;
    private readonly ILogger<IdentityAccountService> _logger;

    public IdentityAccountService(
        HttpClient httpClient,
        IOptions<IdentityServiceOptions> options,
        ILogger<IdentityAccountService> logger)
    {
        _httpClient = httpClient;
        _options = options.Value;
        _logger = logger;
    }

    public async Task<ProvisionedAccount> CreateAccountAsync(
        Guid tenantId,
        Guid businessEntityId,
        string accountType,
        string email,
        string firstName,
        string lastName,
        IReadOnlyCollection<string> roles,
        CancellationToken cancellationToken)
    {
        var accessToken = await GetAccessTokenAsync(cancellationToken);

        using var request = new HttpRequestMessage(
            HttpMethod.Post,
            "api/internal/accounts");

        request.Headers.Authorization = new AuthenticationHeaderValue(
            "Bearer",
            accessToken);

        request.Content = JsonContent.Create(new
        {
            tenantId,
            businessEntityId,
            accountType,
            email,
            firstName,
            lastName,
            roles = roles.ToArray()
        });

        _logger.LogInformation(
            "Creating identity account for {Email} in tenant {TenantId} with roles {Roles}.",
            email,
            tenantId,
            roles);

        using var response = await _httpClient.SendAsync(
            request,
            cancellationToken);
		if (!response.IsSuccessStatusCode)
		{
			_logger.LogError(
				"Failed to create identity account for {Email} in tenant {TenantId} with roles {Roles}." +
				" Status: {StatusCode}  response: {ResponseContent}",
				email,
				tenantId,
				roles,
				response.StatusCode,
				await response.Content.ReadAsStringAsync(cancellationToken));
		}
        await EnsureSuccessfulResponseAsync(
            response,
            "create identity account",
            cancellationToken);

        var result = await response.Content.ReadFromJsonAsync<CreateAccountResponse>(
            cancellationToken: cancellationToken);

        if (result is null)
        {
            throw new InvalidOperationException(
                "Identity service did not return the provisioned account.");
        }

        return new ProvisionedAccount(
            result.UserId,
            result.Email,
            result.TemporaryPassword,
            result.MustChangePassword);
    }

    public async Task DeleteAccountAsync(
        Guid userId,
        CancellationToken cancellationToken)
    {
        await SendWithoutBodyAsync(
            HttpMethod.Delete,
            $"api/internal/accounts/{userId}",
            cancellationToken);
    }

    public async Task DeactivateAccountAsync(
        Guid userId,
        CancellationToken cancellationToken)
    {
        await SendWithoutBodyAsync(
            HttpMethod.Post,
            $"api/internal/accounts/{userId}/deactivate",
            cancellationToken);
    }

    private async Task SendWithoutBodyAsync(
        HttpMethod method,
        string relativeUri,
        CancellationToken cancellationToken)
    {
        var accessToken = await GetAccessTokenAsync(cancellationToken);

        using var request = new HttpRequestMessage(method, relativeUri);
        request.Headers.Authorization = new AuthenticationHeaderValue(
            "Bearer",
            accessToken);

        using var response = await _httpClient.SendAsync(
            request,
            cancellationToken);

        await EnsureSuccessfulResponseAsync(
            response,
            $"call identity endpoint '{relativeUri}'",
            cancellationToken);
    }

    private async Task<string> GetAccessTokenAsync(
        CancellationToken cancellationToken)
    {
        using var tokenRequest = new HttpRequestMessage(
            HttpMethod.Post,
            "connect/token");

        tokenRequest.Content = new FormUrlEncodedContent(
            new Dictionary<string, string>
            {
                ["grant_type"] = "client_credentials",
                ["client_id"] = _options.ClientId,
                ["client_secret"] = _options.ClientSecret,
                ["scope"] = _options.Scope
            });
		Log.Information("Requesting access token from identity service for client_id: {ClientId}" +
			" and scope: {Scope}  client_secret: {ClientSecret}", _options.ClientId, _options.Scope, _options.ClientSecret	);

		using var response = await _httpClient.SendAsync(
            tokenRequest,
            cancellationToken);
		if (!response.IsSuccessStatusCode)
		{
			_logger.LogInformation(
				"Failed to obtain access token from identity service for client_id: {ClientId} and scope: {Scope}. Status: {StatusCode}  response: {ResponseContent}",
				_options.ClientId,
				_options.Scope,
				response.StatusCode,
				await response.Content.ReadAsStringAsync(cancellationToken));
		}


			await EnsureSuccessfulResponseAsync(
            response,
            "obtain identity service access token",
            cancellationToken);

        await using var contentStream = await response.Content.ReadAsStreamAsync(
            cancellationToken);

        using var document = await JsonDocument.ParseAsync(
            contentStream,
            cancellationToken: cancellationToken);

        if (!document.RootElement.TryGetProperty("access_token", out var tokenElement))
        {
            throw new InvalidOperationException(
                "Identity service token response does not contain access_token.");
        }

        var accessToken = tokenElement.GetString();

        if (string.IsNullOrWhiteSpace(accessToken))
        {
            throw new InvalidOperationException(
                "Identity service returned an empty access token.");
        }

        return accessToken;
    }

    private async Task EnsureSuccessfulResponseAsync(
        HttpResponseMessage response,
        string operation,
        CancellationToken cancellationToken)
    {
        if (response.IsSuccessStatusCode)
        {
            return;
        }

        var responseBody = await response.Content.ReadAsStringAsync(
            cancellationToken);

        _logger.LogError(
            "Identity service failed to {Operation}. StatusCode: {StatusCode}. Response: {ResponseBody}",
            operation,
            (int)response.StatusCode,
            responseBody);

        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            throw new InvalidOperationException(
                $"Identity service rejected the service access token while attempting to {operation}.");
        }

        response.EnsureSuccessStatusCode();
    }

    private sealed record CreateAccountResponse(
        Guid UserId,
        string Email,
        string TemporaryPassword,
        bool MustChangePassword);
}
