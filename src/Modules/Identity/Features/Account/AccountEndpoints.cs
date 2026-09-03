using SmartSchool.Application.Identity;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http.Extensions;
using SmartSchool.Modules.Identity.Persistence.Identity;

namespace SmartSchool.Modules.Identity.Features.Account;

public static class AccountEndpoints
{
    public sealed record LoginRequest(string Email, string Password);
    public sealed record LoginResponse(
        string AccessToken,
        string TokenType,
        int ExpiresIn,
        string? RefreshToken,
        string Scope,
        UserSummary User);
    public sealed record UserSummary(
        Guid Id,
        Guid? TenantId,
        string Email,
        string FirstName,
        string LastName,
        string DisplayName,
        string AccountType,
        IReadOnlyList<string> Roles);
    public sealed record ForgotPasswordRequest(string Email);
    public sealed record ResetPasswordRequest(string Email, string Token, string NewPassword);
    public sealed record ChangePasswordRequest(string CurrentPassword, string NewPassword);
    public sealed record RefreshTokenRequest(string RefreshToken);

    public static void MapEndpoints(IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("/api/account").WithTags("Identity - Account");

        group.MapPost("/login", LoginAsync).AllowAnonymous();
        group.MapPost("/forgot-password", ForgotPasswordAsync).AllowAnonymous();
        group.MapPost("/reset-password", ResetPasswordAsync).AllowAnonymous();
        group.MapPost("/change-password", ChangePasswordAsync).RequireAuthorization();
        group.MapPost("/refresh", RefreshAsync).AllowAnonymous();
        group.MapGet("/me", MeAsync).RequireAuthorization();
    }

    private static async Task<IResult> LoginAsync(
        LoginRequest request,
        HttpContext httpContext,
        UserManager<SmartSchoolUser> userManager,
        SignInManager<SmartSchoolUser> signInManager,
        IHttpClientFactory httpClientFactory,
        IConfiguration configuration,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
        {
            return Results.BadRequest(new { message = "Email and password are required." });
        }

        var user = await userManager.FindByEmailAsync(request.Email.Trim());
        if (user is null || !user.IsActive)
        {
            return Results.Json(new { message = "Invalid email or password." }, statusCode: StatusCodes.Status401Unauthorized);
        }

        var passwordResult = await signInManager.CheckPasswordSignInAsync(user, request.Password, lockoutOnFailure: true);
        if (passwordResult.IsLockedOut)
        {
            return Results.Json(new { message = "Account is temporarily locked." }, statusCode: StatusCodes.Status423Locked);
        }
        if (!passwordResult.Succeeded)
        {
            return Results.Json(new { message = "Invalid email or password." }, statusCode: StatusCodes.Status401Unauthorized);
        }

        var clientId = configuration["LoginApiClient:ClientId"] ?? throw new InvalidOperationException("LoginApiClient:ClientId is required.");
        var clientSecret = configuration["LoginApiClient:ClientSecret"];
        if (string.IsNullOrWhiteSpace(clientSecret))
        {
            throw new InvalidOperationException("LoginApiClient:ClientSecret is required.");
        }

        var tokenUrl = configuration["LoginApiClient:TokenEndpoint"] ?? throw new InvalidOperationException("LoginApiClient:TokenEndpoint is required.");
        using var tokenRequest = new HttpRequestMessage(HttpMethod.Post, tokenUrl)
        {
            Content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                ["grant_type"] = "password",
                ["client_id"] = clientId,
                ["client_secret"] = clientSecret,
                ["username"] = user.Email ?? request.Email.Trim(),
                ["password"] = request.Password,
                ["scope"] = "smartschool.api offline_access"
            })
        };

        var client = httpClientFactory.CreateClient("IdentityTokenClient");
        HttpResponseMessage tokenResponse;
        try
        {
            tokenResponse = await client.SendAsync(tokenRequest, cancellationToken);
        }
        catch (HttpRequestException exception)
        {
            return Results.Problem(
                title: "Identity token service is unavailable.",
                detail: $"Could not reach {tokenUrl}. {exception.Message}",
                statusCode: StatusCodes.Status503ServiceUnavailable);
        }
        using (tokenResponse)
        {
        var tokenJson = await tokenResponse.Content.ReadAsStringAsync(cancellationToken);
        if (!tokenResponse.IsSuccessStatusCode)
        {
            return Results.Json(
                new { message = "Authentication failed at the token service.", detail = tokenJson },
                statusCode: (int)tokenResponse.StatusCode);
        }

        using var document = JsonDocument.Parse(tokenJson);
        var root = document.RootElement;
        var roles = (await userManager.GetRolesAsync(user)).ToArray();
        var response = new LoginResponse(
            root.GetProperty("access_token").GetString()!,
            root.TryGetProperty("token_type", out var tokenType) ? tokenType.GetString() ?? "Bearer" : "Bearer",
            root.TryGetProperty("expires_in", out var expiresIn) ? expiresIn.GetInt32() : 0,
            root.TryGetProperty("refresh_token", out var refreshToken) ? refreshToken.GetString() : null,
            root.TryGetProperty("scope", out var scope) ? scope.GetString() ?? string.Empty : string.Empty,
            new UserSummary(user.Id, user.TenantId, user.Email ?? string.Empty, user.FirstName, user.LastName,
                user.DisplayName ?? string.Empty, user.AccountType ?? string.Empty, roles));

            return Results.Ok(response);
        }
    }

    private static async Task<IResult> ForgotPasswordAsync(
        ForgotPasswordRequest request,
        UserManager<SmartSchoolUser> userManager,
        ILoggerFactory loggerFactory)
    {
        var user = await userManager.FindByEmailAsync(request.Email);
        if (user is not null && user.IsActive)
        {
            var token = await userManager.GeneratePasswordResetTokenAsync(user);
            loggerFactory.CreateLogger("PasswordReset")
                .LogInformation("Password reset requested for user {UserId}. Token generated: {Token}", user.Id, token);
        }

        return Results.Accepted(value: new
        {
            message = "If the account exists, password reset instructions will be sent."
        });
    }

    private static async Task<IResult> ResetPasswordAsync(
        ResetPasswordRequest request,
        UserManager<SmartSchoolUser> userManager)
    {
        var user = await userManager.FindByEmailAsync(request.Email);
        if (user is null || !user.IsActive)
        {
            return Results.BadRequest(new { message = "Invalid password reset request." });
        }

        var result = await userManager.ResetPasswordAsync(user, request.Token, request.NewPassword);
        return result.Succeeded
            ? Results.NoContent()
            : Results.ValidationProblem(ToErrors(result));
    }

    private static async Task<IResult> ChangePasswordAsync(
        ChangePasswordRequest request,
        ICurrentUser currentUser,
        UserManager<SmartSchoolUser> userManager)
    {
        var user = await userManager.FindByIdAsync(currentUser.UserId.ToString());
        if (user is null) return Results.Unauthorized();

        var result = await userManager.ChangePasswordAsync(
            user, request.CurrentPassword, request.NewPassword);

        return result.Succeeded
            ? Results.NoContent()
            : Results.ValidationProblem(ToErrors(result));
    }

    private static async Task<IResult> RefreshAsync(RefreshTokenRequest request, IHttpClientFactory factory, IConfiguration configuration, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.RefreshToken)) return Results.BadRequest(new { message = "Refresh token is required." });
        var clientId = configuration["LoginApiClient:ClientId"] ?? throw new InvalidOperationException("LoginApiClient:ClientId is required.");
        var clientSecret = configuration["LoginApiClient:ClientSecret"] ?? throw new InvalidOperationException("LoginApiClient:ClientSecret is required.");
        var tokenUrl = configuration["LoginApiClient:TokenEndpoint"] ?? throw new InvalidOperationException("LoginApiClient:TokenEndpoint configuration is required.");
        using var message = new HttpRequestMessage(HttpMethod.Post, tokenUrl) { Content = new FormUrlEncodedContent(new Dictionary<string,string> {
            ["grant_type"]="refresh_token", ["client_id"]=clientId, ["client_secret"]=clientSecret, ["refresh_token"]=request.RefreshToken }) };
        using var response = await factory.CreateClient("IdentityTokenClient").SendAsync(message, cancellationToken);
        return Results.Content(await response.Content.ReadAsStringAsync(cancellationToken), "application/json", statusCode:(int)response.StatusCode);
    }
    private static async Task<IResult> MeAsync(ICurrentUser currentUser, UserManager<SmartSchoolUser> manager)
    {
        var user = await manager.FindByIdAsync(currentUser.UserId.ToString());
        if (user is null) return Results.Unauthorized();
        var roles=(await manager.GetRolesAsync(user)).ToArray();
        return Results.Ok(new UserSummary(user.Id,user.TenantId,user.Email??string.Empty,
            user.FirstName,user.LastName,user.DisplayName??string.Empty,user.AccountType??string.Empty,roles));
    }

    private static Dictionary<string, string[]> ToErrors(IdentityResult result) =>
        result.Errors.GroupBy(x => x.Code)
            .ToDictionary(x => x.Key, x => x.Select(e => e.Description).ToArray());
}
