using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using SmartSchool.Application.Identity;

namespace SmartSchool.Infrastructure.Identity;

public sealed class IdentityAccountService(
	HttpClient httpClient,
	IdentityServiceOptions options) : IIdentityAccountService
{
	public async Task<ProvisionedAccount> CreateAccountAsync(
		Guid tenantId, Guid businessEntityId, string accountType,
		string email, string firstName, string lastName,
		IReadOnlyCollection<string> roles, CancellationToken cancellationToken)
	{
		using var request = new HttpRequestMessage(HttpMethod.Post, "api/internal/accounts");
		request.Headers.Authorization = new AuthenticationHeaderValue(
			"Bearer", await GetAccessTokenAsync(cancellationToken));
		request.Content = JsonContent.Create(new
		{
			tenantId, businessEntityId, accountType, email, firstName, lastName,
			roles = roles.ToArray()
		});

		using var response = await httpClient.SendAsync(request, cancellationToken);
		response.EnsureSuccessStatusCode();

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

	public Task DeleteAccountAsync(Guid userId, CancellationToken cancellationToken) =>
		SendWithoutBodyAsync(HttpMethod.Delete, $"api/internal/accounts/{userId}", cancellationToken);

	public Task DeactivateAccountAsync(Guid userId, CancellationToken cancellationToken) =>
		SendWithoutBodyAsync(HttpMethod.Post, $"api/internal/accounts/{userId}/deactivate", cancellationToken);

	private async Task SendWithoutBodyAsync(
		HttpMethod method, string uri, CancellationToken cancellationToken)
	{
		using var request = new HttpRequestMessage(method, uri);
		request.Headers.Authorization = new AuthenticationHeaderValue(
			"Bearer", await GetAccessTokenAsync(cancellationToken));
		using var response = await httpClient.SendAsync(request, cancellationToken);
		response.EnsureSuccessStatusCode();
	}

	private async Task<string> GetAccessTokenAsync(CancellationToken cancellationToken)
	{
		using var tokenRequest = new HttpRequestMessage(HttpMethod.Post, "connect/token")
		{
			Content = new FormUrlEncodedContent(new Dictionary<string,string>
			{
				["grant_type"] = "client_credentials",
				["client_id"] = options.ClientId,
				["client_secret"] = options.ClientSecret,
				["scope"] = options.Scope
			})
		};

		using var response = await httpClient.SendAsync(tokenRequest, cancellationToken);
		response.EnsureSuccessStatusCode();
		using var document = JsonDocument.Parse(
			await response.Content.ReadAsStringAsync(cancellationToken));
		return document.RootElement.GetProperty("access_token").GetString()
			?? throw new InvalidOperationException("Identity service returned no access token.");
	}

	private sealed record CreateAccountResponse(
		Guid UserId,
		string Email,
		string TemporaryPassword,
		bool MustChangePassword);
}
