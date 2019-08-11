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
    }
}
