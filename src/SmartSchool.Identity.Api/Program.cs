using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using SmartSchool.Identity.Api;
using SmartSchool.Identity.Api.Observability;
using SmartSchool.Modules.Identity;
using SmartSchool.SharedKernel.Constants;

var builder = WebApplication.CreateBuilder(args);
builder.AddSmartSchoolSerilog("SmartSchool.Identity.Api");

builder.Services.AddRazorPages();
builder.Services.AddSmartSchoolObservability(builder.Configuration, "SmartSchool.Identity.Api");
builder.Services.AddIdentityModule(builder.Configuration);

builder.Services
    .AddOptions<InternalApiAuthenticationOptions>()
    .Bind(builder.Configuration.GetSection(InternalApiAuthenticationOptions.SectionName))
    .Validate(options => !string.IsNullOrWhiteSpace(options.Authority),
        "InternalApiAuthentication:Authority is required.")
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
                ValidIssuer = internalApiAuthentication.Authority.TrimEnd('/'),
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
    options.AddPolicy("AdminOnly", policy =>
    {
        policy.RequireRole(SmartSchoolRoles.SuperAdmin, "SchoolAdmin", "Principal", "Admin");
    });

    options.AddPolicy("SuperAdminOnly", policy =>
    {
        policy.RequireRole(SmartSchoolRoles.SuperAdmin);
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
    app.UseHttpsRedirection();
}
app.UseStaticFiles();
app.UseRouting();
app.UseCors("Portal");
app.UseTelemetryResponseHeaders();
app.UseIdentityServer();
app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();
app.MapIdentityServerEndpoints();
app.MapUiErrorEndpoints();

app.MapGet("/", () => Results.Ok(new
{
	service = "SmartSchool.Identity.Api",
	status = "Running"
}));

app.Run();
