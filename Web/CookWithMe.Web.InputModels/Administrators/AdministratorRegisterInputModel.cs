namespace CookWithMe.Web.InputModels.Administrators
{
    using System.ComponentModel.DataAnnotations;

    using CookWithMe.Common;
    using CookWithMe.Services.Mapping;
    using CookWithMe.Services.Models;

    public class AdministratorRegisterInputModel : IMapTo<AdministratorServiceModel>
    {
        [Display(Name = "Username")]
        [Required(ErrorMessage = ErrorMessages.RequiredErrorMessage)]
        [MaxLength(256, ErrorMessage = ErrorMessages.MaxLengthErrorMessage)]
        public string Username { get; set; }

        [Display(Name = "Full Name")]
        [Required(ErrorMessage = ErrorMessages.RequiredErrorMessage)]
        [StringLength(50, ErrorMessage = ErrorMessages.StringLengthErrorMessage, MinimumLength = 3)]
        public string FullName { get; set; }

        [Display(Name = "Email")]
        [Required(ErrorMessage = ErrorMessages.RequiredErrorMessage)]
        [StringLength(256, ErrorMessage = ErrorMessages.StringLengthErrorMessage, MinimumLength = 3)]
        [EmailAddress]
        public string Email { get; set; }

        [Display(Name = "Password")]
        [Required(ErrorMessage = ErrorMessages.RequiredErrorMessage)]
        [StringLength(100, ErrorMessage = ErrorMessages.StringLengthErrorMessage, MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
