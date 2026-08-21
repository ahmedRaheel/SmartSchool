using SmartSchool.Modules.Identity;

var builder = WebApplication.CreateBuilder(args);

// ---------------------------------------------------------
// Services
// ---------------------------------------------------------

builder.Services.AddRazorPages();

builder.Services.AddIdentityModule(builder.Configuration);

builder.Services.AddAuthorization(options =>
{
	options.AddPolicy(
		"AdminOnly",
		policy =>
			policy.RequireRole(
				"SuperAdmin",
				"Principal",
				"Admin"));

	options.AddPolicy(
		"SmartSchoolApi",
		policy =>
			policy.RequireClaim(
				"scope",
				"smartschool.identity.manage"));
});

builder.Services.AddCors(options =>
{
	options.AddPolicy(
		"SmartSchoolPortal",
		policy =>
		{
			policy
				.WithOrigins(
					"http://localhost:5173",
					"http://127.0.0.1:5173")
				.AllowAnyHeader()
				.AllowAnyMethod()
				.AllowCredentials();
		});
});

// ---------------------------------------------------------
// Build
// ---------------------------------------------------------

var app = builder.Build();

// ---------------------------------------------------------
// Development seed
// ---------------------------------------------------------

if (app.Environment.IsDevelopment())
{
	using var scope = app.Services.CreateScope();

	var identitySeeder = scope.ServiceProvider
		.GetRequiredService<
			SmartSchool.Modules.Identity.Server.IdentityDataSeeder>();

	await identitySeeder.SeedAsync();

	var duendeSeeder = scope.ServiceProvider
		.GetRequiredService<
			SmartSchool.Modules.Identity.Server.DuendeConfigurationSeeder>();

	await duendeSeeder.SeedAsync();
}

// ---------------------------------------------------------
// Middleware
// ---------------------------------------------------------

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseCors("SmartSchoolPortal");

app.UseIdentityServer();

app.UseAuthentication();

app.UseAuthorization();

// ---------------------------------------------------------
// Endpoints
// ---------------------------------------------------------

app.MapRazorPages();

app.MapIdentityServerEndpoints();

app.MapGet(
	"/",
	() => Results.Ok(new
	{
		service = "SmartSchool.Identity.Api",
		status = "Running"
	}));

app.Run();
