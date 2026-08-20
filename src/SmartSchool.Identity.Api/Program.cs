using SmartSchool.Modules.Identity;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddIdentityModule(builder.Configuration);

builder.Services.AddAuthorization(options =>
{
	options.AddPolicy("AdminOnly", policy =>
		policy.RequireRole("SuperAdmin", "Principal", "Admin"));

	// SmartSchool.Api obtains a client-credentials token with this scope.
	options.AddPolicy("SmartSchoolApi", policy =>
		policy.RequireClaim("scope", "smartschool.identity.manage"));
});

var app = builder.Build();

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseIdentityServer();
app.UseAuthorization();

app.MapRazorPages();
app.MapIdentityEndpoints();

app.MapGet("/", () => Results.Ok(new
{
	service = "SmartSchool.Identity.Api",
	status = "Running"
}));

app.Run();
