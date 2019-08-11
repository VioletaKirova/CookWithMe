namespace CookWithMe.Web.Areas.Identity.Pages.Account
{
    using System.ComponentModel.DataAnnotations;
    using System.Text.Encodings.Web;
    using System.Threading.Tasks;

    using CookWithMe.Common;
    using CookWithMe.Data.Models;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Identity.UI.Services;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.Extensions.Logging;

    [AllowAnonymous]
#pragma warning disable SA1649 // File name should match first type name
    public class RegisterModel : PageModel
#pragma warning restore SA1649 // File name should match first type name
    {
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ILogger<RegisterModel> logger;
        private readonly IEmailSender emailSender;

        public RegisterModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.logger = logger;
            this.emailSender = emailSender;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public void OnGet(string returnUrl = null)
        {
            this.ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? "/Users/AddAdditionalInfo";
            if (this.ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = this.Input.Username,
                    FullName = this.Input.FullName,
                    Email = this.Input.Email,
                };
                var result = await this.userManager.CreateAsync(user, this.Input.Password);
                if (result.Succeeded)
                {
                    this.logger.LogInformation("User created a new account with password.");

                    var code = await this.userManager.GenerateEmailConfirmationTokenAsync(user);
                    var callbackUrl = this.Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { userId = user.Id, code = code },
                        protocol: this.Request.Scheme);

                    await this.emailSender.SendEmailAsync(
                        this.Input.Email,
                        "Confirm your email",
                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    await this.userManager.AddToRoleAsync(user, GlobalConstants.UserRoleName);

                    await this.signInManager.SignInAsync(user, isPersistent: false);
                    return this.LocalRedirect(returnUrl);
                }

                foreach (var error in result.Errors)
                {
                    this.ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return this.Page();
        }

        public class InputModel
        {
            [Display(Name = "Username")]
            [Required(ErrorMessage = AttributesErrorMessages.RequiredErrorMessage)]
            [MaxLength(256, ErrorMessage = AttributesErrorMessages.MaxLengthErrorMessage)]
            public string Username { get; set; }

            [Display(Name = "Full Name")]
            [Required(ErrorMessage = AttributesErrorMessages.RequiredErrorMessage)]
            [StringLength(50, ErrorMessage = AttributesErrorMessages.StringLengthErrorMessage, MinimumLength = 3)]
            public string FullName { get; set; }

            [Display(Name = "Email")]
            [Required(ErrorMessage = AttributesErrorMessages.RequiredErrorMessage)]
            [StringLength(256, ErrorMessage = AttributesErrorMessages.StringLengthErrorMessage, MinimumLength = 3)]
            [EmailAddress]
            public string Email { get; set; }

            [Display(Name = "Password")]
            [Required(ErrorMessage = AttributesErrorMessages.RequiredErrorMessage)]
            [StringLength(100, ErrorMessage = AttributesErrorMessages.StringLengthErrorMessage, MinimumLength = 6)]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [Display(Name = "Confirm Password")]
            [Compare("Password", ErrorMessage = AttributesErrorMessages.CompareErrorMessage)]
            [DataType(DataType.Password)]
            public string ConfirmPassword { get; set; }
        }
    }
}
