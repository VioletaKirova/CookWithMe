namespace CookWithMe.Web.InputModels.Administrators.Register
{
    using System.ComponentModel.DataAnnotations;

    using CookWithMe.Common;
    using CookWithMe.Services.Mapping;
    using CookWithMe.Services.Models.Administrators;

    public class AdministratorRegisterInputModel : IMapTo<AdministratorServiceModel>
    {
        [Display(Name = "Username")]
        [Required(ErrorMessage = AttributesErrorMessages.RequiredErrorMessage)]
        [MaxLength(AttributesConstraints.UsernameMaxLength, ErrorMessage = AttributesErrorMessages.MaxLengthErrorMessage)]
        public string Username { get; set; }

        [Display(Name = "Full Name")]
        [Required(ErrorMessage = AttributesErrorMessages.RequiredErrorMessage)]
        [StringLength(AttributesConstraints.FullNameMaxLength, ErrorMessage = AttributesErrorMessages.StringLengthErrorMessage, MinimumLength = AttributesConstraints.FullNameMinLength)]
        public string FullName { get; set; }

        [Display(Name = "Email")]
        [Required(ErrorMessage = AttributesErrorMessages.RequiredErrorMessage)]
        [StringLength(AttributesConstraints.EmailMaxLength, ErrorMessage = AttributesErrorMessages.StringLengthErrorMessage, MinimumLength = AttributesConstraints.FullNameMinLength)]
        [EmailAddress]
        public string Email { get; set; }

        [Display(Name = "Password")]
        [Required(ErrorMessage = AttributesErrorMessages.RequiredErrorMessage)]
        [StringLength(AttributesConstraints.PasswordMaxLength, ErrorMessage = AttributesErrorMessages.StringLengthErrorMessage, MinimumLength = AttributesConstraints.PasswordMinLength)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
