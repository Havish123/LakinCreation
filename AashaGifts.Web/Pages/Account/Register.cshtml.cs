using AashaGifts.Web.Enums;
using AashaGifts.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace AashaGifts.Web.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public RegisterModel(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [BindProperty]
        public RegisterInput Input { get; set; } = new();

        public string? ErrorMessage { get; set; }

        public class RegisterInput
        {
            [Required]
            public string Username { get; set; } = "";
            [Required]
            [EmailAddress]
            public string Email { get; set; } = "";
            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; } = "";
            [Required]
            [Compare("Password")]
            [DataType(DataType.Password)]
            public string ConfirmPassword { get; set; } = "";
        }

        public void OnGet() { }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            var user = new ApplicationUser
            {
                UserName = Input.Username,
                Email = Input.Email,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, Input.Password);

            if (result.Succeeded)
            {
                // Add role *after* user is created
                var roleResult = await _userManager.AddToRoleAsync(user, UserRoles.Customer.GetEnumDescription());
                if (!roleResult.Succeeded)
                {
                    ErrorMessage = string.Join(" ", roleResult.Errors.Select(e => e.Description));
                    return Page();
                }

                await _signInManager.SignInAsync(user, false);
                return RedirectToPage("/Index");
            }

            ErrorMessage = string.Join(" ", result.Errors.Select(e => e.Description));
            return Page();
        }
    }
}