using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SmartSchool.Modules.Identity.Infrastructure.Identity;

namespace SmartSchool.Identity.Api.Pages.Account;
public sealed class ForgotPasswordModel(UserManager<SmartSchoolUser> users, ILogger<ForgotPasswordModel> logger) : PageModel
{
	[BindProperty] public string Email { get; set; } = string.Empty;
	public bool Submitted { get; private set; }
	public async Task OnPostAsync()
	{
		var user=await users.FindByEmailAsync(Email);
		if(user is not null && user.IsActive)
		{
			var token=await users.GeneratePasswordResetTokenAsync(user);
			logger.LogInformation("Reset token generated for {UserId}: {Token}",user.Id,token);
		}
		Submitted=true;
	}
}
