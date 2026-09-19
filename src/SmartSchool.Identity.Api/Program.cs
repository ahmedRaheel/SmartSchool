using System.Threading.RateLimiting;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using SmartSchool.Identity.Api;
using SmartSchool.Identity.Api.Observability;
using SmartSchool.Modules.Identity;
using SmartSchool.SharedKernel.Constants;

var builder = WebApplication.CreateBuilder(args);
builder.AddSmartSchoolSerilog("SmartSchool.Identity.Api");

if (!builder.Environment.IsDevelopment())
{
    ValidateProductionConfiguration(builder.Configuration);
}

builder.Services.AddRazorPages();
builder.Services.AddSmartSchoolObservability(builder.Configuration, "SmartSchool.Identity.Api");
builder.Services.AddIdentityModule(builder.Configuration);
builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
    options.AddPolicy("authentication", httpContext =>
        RateLimitPartition.GetFixedWindowLimiter(
            httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown",
            _ => new FixedWindowRateLimiterOptions
            {
                PermitLimit = 10,
                Window = TimeSpan.FromMinutes(1),
                QueueLimit = 0,
                AutoReplenishment = true
            }));
    options.AddPolicy("password-reset", httpContext =>
        RateLimitPartition.GetFixedWindowLimiter(
            httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown",
            _ => new FixedWindowRateLimiterOptions
            {
                PermitLimit = 5,
                Window = TimeSpan.FromMinutes(10),
                QueueLimit = 0,
                AutoReplenishment = true
            }));
});

builder.Services
    .AddOptions<InternalApiAuthenticationOptions>()
    .Bind(builder.Configuration.GetSection(InternalApiAuthenticationOptions.SectionName))
    .Validate(options => !string.IsNullOrWhiteSpace(options.Authority),
        "InternalApiAuthentication:Authority is required.")
    .Validate(options => !string.IsNullOrWhiteSpace(options.ValidIssuer),
        "InternalApiAuthentication:ValidIssuer is required.")
    .Validate(options => !string.IsNullOrWhiteSpace(options.RequiredScope),
        "InternalApiAuthentication:RequiredScope is required.")
    .ValidateOnStart();

var internalApiAuthentication = builder.Configuration
    .GetRequiredSection(InternalApiAuthenticationOptions.SectionName)
    .Get<InternalApiAuthenticationOptions>()
    ?? throw new InvalidOperationException(
        "InternalApiAuthentication configuration is required.");

builder.Services
    .AddAuthentication()
    .AddJwtBearer(
        InternalApiAuthenticationOptions.SchemeName,
        options =>
        {
            options.Authority = internalApiAuthentication.Authority;
            options.RequireHttpsMetadata = internalApiAuthentication.RequireHttpsMetadata;
            options.MapInboundClaims = false;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = internalApiAuthentication.ValidIssuer.TrimEnd('/'),
                ValidateAudience = false,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ClockSkew = TimeSpan.FromMinutes(1)
            };

            options.Events = new JwtBearerEvents
            {
                OnAuthenticationFailed = context =>
                {
                    var logger = context.HttpContext.RequestServices
                        .GetRequiredService<ILoggerFactory>()
                        .CreateLogger(InternalApiAuthenticationOptions.SchemeName);

                    logger.LogError(
                        context.Exception,
                        "Internal API bearer authentication failed.");

                    return Task.CompletedTask;
                },
                OnTokenValidated = context =>
                {
                    var logger = context.HttpContext.RequestServices
                        .GetRequiredService<ILoggerFactory>()
                        .CreateLogger(InternalApiAuthenticationOptions.SchemeName);

                    logger.LogInformation(
                        "Internal API bearer token validated for client {ClientId}.",
                        context.Principal?.FindFirst("client_id")?.Value);

                    return Task.CompletedTask;
                }
            };
        });

builder.Services.AddAuthorization(options =>
{
    // Identity hosts Razor/cookie login pages and bearer-protected JSON APIs in
    // the same process. API endpoints use the default authorization policy, so
    // make that policy explicitly authenticate bearer tokens rather than the
    // ASP.NET Identity application cookie. This does not change SignInManager's
    // cookie scheme used by the Razor login flow.
    options.DefaultPolicy = new Microsoft.AspNetCore.Authorization.AuthorizationPolicyBuilder(
            InternalApiAuthenticationOptions.SchemeName)
        .RequireAuthenticatedUser()
        .Build();

    options.AddPolicy("AdminOnly", policy =>
    {
        policy.AddAuthenticationSchemes(
            InternalApiAuthenticationOptions.SchemeName);
        policy.RequireAuthenticatedUser();
        policy.RequireRole(
            SmartSchoolRoles.SuperAdmin,
            "SchoolAdmin",
            "Principal",
            "Admin");
    });

    options.AddPolicy("SuperAdminOnly", policy =>
    {
        policy.AddAuthenticationSchemes(
            InternalApiAuthenticationOptions.SchemeName);
        policy.RequireAuthenticatedUser();
        policy.RequireRole(SmartSchoolRoles.SuperAdmin);
    });

    options.AddPolicy(SmartSchoolPolicies.Impersonation, policy =>
    {
        policy.AddAuthenticationSchemes(
            InternalApiAuthenticationOptions.SchemeName);
        policy.RequireAuthenticatedUser();
        policy.RequireRole(
            SmartSchoolRoles.SuperAdmin,
            SmartSchoolRoles.SuperOwner,
            SmartSchoolRoles.Tenant,
            SmartSchoolRoles.TenantAdmin,
            SmartSchoolRoles.Owner,
            SmartSchoolRoles.Admin,
            SmartSchoolRoles.AdminOfficer);
    });

    options.AddPolicy("SmartSchoolApi", policy =>
    {
        policy.AddAuthenticationSchemes(
            InternalApiAuthenticationOptions.SchemeName);
        policy.RequireAuthenticatedUser();
        policy.RequireClaim(
            "scope",
            internalApiAuthentication.RequiredScope);
    });
});

var portalOrigins = builder.Configuration
    .GetSection("Cors:PortalOrigins")
    .Get<string[]>()
    ?? throw new InvalidOperationException("Cors:PortalOrigins configuration is required.");

builder.Services.AddCors(options => options.AddPolicy("Portal", policy => policy
    .WithOrigins(portalOrigins)
    .AllowAnyHeader().AllowAnyMethod().AllowCredentials().WithExposedHeaders("X-Correlation-ID", "X-Trace-Id")));

var app = builder.Build();

app.UseSerilogRequestLogging(options =>
{
    options.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
    {
        diagnosticContext.Set("TraceId", System.Diagnostics.Activity.Current?.TraceId.ToString());
        diagnosticContext.Set("CorrelationId", httpContext.TraceIdentifier);
    };
});

if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var identitySeeder = scope.ServiceProvider
        .GetRequiredService<SmartSchool.Modules.Identity.Server.IdentityDataSeeder>();
    await identitySeeder.SeedAsync();

    var duendeSeeder = scope.ServiceProvider
        .GetRequiredService<SmartSchool.Modules.Identity.Server.DuendeConfigurationSeeder>();
    await duendeSeeder.SeedAsync();
}

if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
    app.UseHttpsRedirection();
}
app.UseStaticFiles();
app.UseRouting();
app.UseRateLimiter();
app.UseCors("Portal");
app.UseTelemetryResponseHeaders();
app.UseIdentityServer();
app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();
app.MapIdentityServerEndpoints();
app.MapSmokeTestIdentityFixtureEndpoints(builder.Configuration, app.Environment);
app.MapUiErrorEndpoints();

app.MapGet("/", () => Results.Ok(new
{
    service = "SmartSchool.Identity.Api",
    status = "Running"
}));

app.Run();


static void ValidateProductionConfiguration(IConfiguration configuration)
{
    static bool IsSecureAbsoluteUrl(string? value) =>
        Uri.TryCreate(value, UriKind.Absolute, out var uri) &&
        uri.Scheme.Equals(Uri.UriSchemeHttps, StringComparison.OrdinalIgnoreCase) &&
        !uri.IsLoopback;

    var connectionString = configuration.GetConnectionString("SmartSchool");
    if (string.IsNullOrWhiteSpace(connectionString) ||
        connectionString.Contains("CHANGE_ME", StringComparison.OrdinalIgnoreCase) ||
        connectionString.Contains("postgres123", StringComparison.OrdinalIgnoreCase))
    {
        throw new InvalidOperationException("A production SmartSchool connection string must be supplied through secure configuration.");
    }

    if (configuration.GetValue<bool>("DuendeIdentityServer:UseDeveloperSigningCredential"))
        throw new InvalidOperationException("Developer IdentityServer signing credentials are not allowed outside Development.");

    if (!IsSecureAbsoluteUrl(configuration["DuendeIdentityServer:IssuerUri"]))
        throw new InvalidOperationException("DuendeIdentityServer:IssuerUri must be a non-loopback HTTPS URL in production.");
    if (!IsSecureAbsoluteUrl(configuration["DuendeIdentityServer:PortalUrl"]))
        throw new InvalidOperationException("DuendeIdentityServer:PortalUrl must be a non-loopback HTTPS URL in production.");

    var serviceSecret = configuration["SmartSchoolApiClient:ClientSecret"];
    var loginSecret = configuration["LoginApiClient:ClientSecret"];
    if (string.IsNullOrWhiteSpace(serviceSecret) || serviceSecret.Contains("change-me", StringComparison.OrdinalIgnoreCase))
        throw new InvalidOperationException("SmartSchoolApiClient:ClientSecret must be provided securely in production.");
    if (string.IsNullOrWhiteSpace(loginSecret) || loginSecret.Contains("change-me", StringComparison.OrdinalIgnoreCase))
        throw new InvalidOperationException("LoginApiClient:ClientSecret must be provided securely in production.");

    var tokenEndpoint = configuration["LoginApiClient:TokenEndpoint"];
    if (!Uri.TryCreate(tokenEndpoint, UriKind.Absolute, out var tokenUri) ||
        (!tokenUri.Scheme.Equals(Uri.UriSchemeHttps, StringComparison.OrdinalIgnoreCase) && !tokenUri.IsLoopback))
    {
        throw new InvalidOperationException(
            "LoginApiClient:TokenEndpoint must be HTTPS or a loopback endpoint in production.");
    }

    if (!IsSecureAbsoluteUrl(configuration["InternalApiAuthentication:Authority"]))
        throw new InvalidOperationException("InternalApiAuthentication:Authority must be a non-loopback HTTPS URL in production.");

    var portalOrigins = configuration.GetSection("Cors:PortalOrigins").Get<string[]>() ?? [];
    if (portalOrigins.Length == 0 || portalOrigins.Any(origin => !IsSecureAbsoluteUrl(origin)))
        throw new InvalidOperationException("Cors:PortalOrigins must contain only non-loopback HTTPS origins in production.");

    var smtpHost = configuration["PasswordResetEmail:SmtpHost"];
    var fromEmail = configuration["PasswordResetEmail:FromEmail"];
    if (string.IsNullOrWhiteSpace(smtpHost) || string.IsNullOrWhiteSpace(fromEmail))
        throw new InvalidOperationException("PasswordResetEmail SMTP host and sender address are required in production.");

    var smtpUsername = configuration["PasswordResetEmail:Username"];
    var smtpPassword = configuration["PasswordResetEmail:Password"];
    if (string.IsNullOrWhiteSpace(smtpUsername) != string.IsNullOrWhiteSpace(smtpPassword))
        throw new InvalidOperationException("PasswordResetEmail username and password must either both be supplied or both be omitted.");

    if (configuration.GetValue<bool>("BootstrapSuperAdmin:Enabled"))
        throw new InvalidOperationException("BootstrapSuperAdmin must be disabled in production after initial provisioning.");
}
