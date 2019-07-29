namespace CookWithMe.Web.InputModels.Administrators
{
    using System.ComponentModel.DataAnnotations;

    using CookWithMe.Services.Mapping;
    using CookWithMe.Services.Models;

    public class AdministratorRegisterInputModel : IMapTo<AdministratorServiceModel>
    {
        [Required]
        [Display(Name = "Username")]
        public string Username { get; set; }

        [Required]
        [Display(Name = "FullName")]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 3)]
        public string FullName { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
    }
}
