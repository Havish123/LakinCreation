using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace AashaGifts.Web.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly SignInManager<AashaGifts.Web.Models.ApplicationUser> _signInManager;

        public LoginModel(SignInManager<AashaGifts.Web.Models.ApplicationUser> signInManager)
        {
            _signInManager = signInManager;
        }

        [BindProperty]
        public LoginInput Input { get; set; } = new();

        public string? ErrorMessage { get; set; }

        public class LoginInput
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; } = "";
            [Required]
            public string Password { get; set; } = "";
        }

        public void OnGet() { }

        public async Task<IActionResult> OnPostAsync(string? returnUrl = null)
        {
            if (!ModelState.IsValid) return Page();

            var result = await _signInManager.PasswordSignInAsync(Input.Email, Input.Password, false, false);
            if (result.Succeeded)
            {
                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    return Redirect(returnUrl);

                return RedirectToPage("/Index");
            }
            ErrorMessage = "Invalid login attempt.";
            return Page();
        }
    }
}