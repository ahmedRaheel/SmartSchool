using Serilog;
using SmartSchool.Identity.Api.Observability;
using SmartSchool.Modules.Identity;

var builder = WebApplication.CreateBuilder(args);
builder.AddSmartSchoolSerilog("SmartSchool.Identity.Api");

builder.Services.AddRazorPages();
builder.Services.AddSmartSchoolObservability(builder.Configuration, "SmartSchool.Identity.Api");
builder.Services.AddIdentityModule(builder.Configuration);

builder.Services.AddAuthorization(options =>
{
	options.AddPolicy("AdminOnly", policy =>
        policy.RequireRole("SuperAdmin", "SchoolAdmin", "Principal", "Admin"));
    options.AddPolicy("SuperAdminOnly", policy =>
        policy.RequireRole("SuperAdmin"));

	// SmartSchool.Api obtains a client-credentials token with this scope.
	options.AddPolicy("SmartSchoolApi", policy =>
		policy.RequireClaim("scope", "smartschool.identity.manage"));
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
