using Duende.IdentityServer.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SmartSchool.Modules.Identity.Infrastructure.Identity;

namespace SmartSchool.Identity.Api.Pages.Account;

public sealed class LoginModel(
    SignInManager<SmartSchoolUser> signInManager,
    IIdentityServerInteractionService interaction) : PageModel
{
    [BindProperty] public string Email { get; set; } = string.Empty;
    [BindProperty] public string Password { get; set; } = string.Empty;
    [BindProperty] public bool RememberLogin { get; set; }
    [BindProperty(SupportsGet = true)] public string? ReturnUrl { get; set; }
    public string? ErrorMessage { get; private set; }

    public async Task<IActionResult> OnPostAsync()
    {
        var result = await signInManager.PasswordSignInAsync(
            Email, Password, RememberLogin, lockoutOnFailure: true);

        if (!result.Succeeded)
        {
            ErrorMessage = result.IsLockedOut ? "Account is locked." : "Invalid email or password.";
            return Page();
        }

        if (!string.IsNullOrWhiteSpace(ReturnUrl) && interaction.IsValidReturnUrl(ReturnUrl))
            return LocalRedirect(ReturnUrl);

        return Redirect("/");
    }
}
