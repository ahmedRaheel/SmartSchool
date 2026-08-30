using Microsoft.AspNetCore.Identity;
using SmartSchool.Modules.Identity.Infrastructure.Identity;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Identity.Server;

public sealed class IdentityDataSeeder(
	RoleManager<SmartSchoolRole> roleManager,
	UserManager<SmartSchoolUser> userManager,
	IConfiguration configuration)
{
	private static readonly string[] Roles =
	[
		SmartSchoolRoles.SuperAdmin, "SchoolAdmin", "Admin", "Principal", SmartSchoolRoles.Teacher, SmartSchoolRoles.Parent, SmartSchoolRoles.Student,
		"Staff", SmartSchoolRoles.Driver, "Examiner", "Exam", "Academics", "Finance", "HR", "Transport"
	];

	public async Task SeedAsync()
	{
		await SeedRolesAsync();
		await SeedSuperAdminAsync();
	}

	private async Task SeedRolesAsync()
	{
		foreach (var roleName in Roles)
		{
			if (await roleManager.RoleExistsAsync(roleName))
			{
				continue;
			}

			var result = await roleManager.CreateAsync(new SmartSchoolRole
			{
				Id = Guid.NewGuid(),
				Name = roleName,
				Description = $"SmartSchool {roleName} role",
				IsSystemRole = true
			});

			EnsureSucceeded(result, $"create role '{roleName}'");
		}
	}

	private async Task SeedSuperAdminAsync()
	{
		if (!configuration.GetValue<bool>("BootstrapSuperAdmin:Enabled"))
		{
			return;
		}

		var email = configuration["BootstrapSuperAdmin:Email"];
		var password = configuration["BootstrapSuperAdmin:Password"];

		if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
		{
			throw new InvalidOperationException(
				"BootstrapSuperAdmin Email and Password are required when bootstrap is enabled.");
		}

		var user = await userManager.FindByEmailAsync(email);
		if (user is null)
		{
			var firstName = configuration["BootstrapSuperAdmin:FirstName"] ?? "SmartSchool";
			var lastName = configuration["BootstrapSuperAdmin:LastName"] ?? SmartSchoolRoles.SuperAdmin;

			user = new SmartSchoolUser
			{
				Id = Guid.NewGuid(),
				TenantId = null,
				BusinessEntityId = null,
				AccountType = SmartSchoolRoles.SuperAdmin,
				UserName = email,
				Email = email,
				EmailConfirmed = true,
				FirstName = firstName,
				LastName = lastName,
				DisplayName = $"{firstName} {lastName}",
				IsActive = true
			};

			EnsureSucceeded(
				await userManager.CreateAsync(user, password),
				"create bootstrap SuperAdmin");
		}

		if (!await userManager.IsInRoleAsync(user, SmartSchoolRoles.SuperAdmin))
		{
			EnsureSucceeded(
				await userManager.AddToRoleAsync(user, SmartSchoolRoles.SuperAdmin),
				"assign SuperAdmin role");
		}
	}

	private static void EnsureSucceeded(IdentityResult result, string operation)
	{
		if (result.Succeeded) return;
		var errors = string.Join("; ", result.Errors.Select(x => $"{x.Code}: {x.Description}"));
		throw new InvalidOperationException($"Failed to {operation}. {errors}");
	}
}
