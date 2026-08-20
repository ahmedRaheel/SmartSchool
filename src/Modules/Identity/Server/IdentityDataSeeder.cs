using Microsoft.AspNetCore.Identity;
using SmartSchool.Modules.Identity.Persistence.Identity;

namespace SmartSchool.Modules.Identity.Server;

public sealed class IdentityDataSeeder(
	RoleManager<SmartSchoolRole> roleManager,
	UserManager<SmartSchoolUser> userManager,
	IConfiguration configuration)
{
	private static readonly string[] Roles = ["SuperAdmin","Principal","Admin","Teacher","Parent","Student","Exam","Academics","Finance","HR","Transport"];

	public async Task SeedAsync()
	{
		foreach(var roleName in Roles)
		{
			if(await roleManager.RoleExistsAsync(roleName)) continue;
			await roleManager.CreateAsync(new SmartSchoolRole {
				Id=Guid.NewGuid(), Name=roleName, Description=$"SmartSchool {roleName} role", IsSystemRole=true
			});
		}

		var email=configuration["DuendeIdentityServer:SeedAdmin:Email"];
		var password=configuration["DuendeIdentityServer:SeedAdmin:Password"];
		if(string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password)) return;
		if(await userManager.FindByEmailAsync(email) is not null) return;

		var user=new SmartSchoolUser {
			Id=Guid.NewGuid(), TenantId=Guid.Parse("11111111-1111-1111-1111-111111111111"),
			UserName=email, Email=email, EmailConfirmed=true, FirstName="System", LastName="Administrator", DisplayName="System Administrator"
		};
		var created=await userManager.CreateAsync(user,password);
		if(created.Succeeded) await userManager.AddToRoleAsync(user,"SuperAdmin");
	}
}
