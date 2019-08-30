namespace CookWithMe.Web.InputModels.Users.AddAdditionalInfo
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using AutoMapper;

    using CookWithMe.Common;
    using CookWithMe.Services.Mapping;
    using CookWithMe.Services.Models.Lifestyles;
    using CookWithMe.Services.Models.Users;
    using CookWithMe.Web.ViewModels.Users.AdditionalInfo;

    using Microsoft.AspNetCore.Http;

    public class UserAddAdditionalInfoInputModel : IMapTo<UserAdditionalInfoServiceModel>, IHaveCustomMappings
    {
        public UserAddAdditionalInfoInputModel()
        {
            this.AllergenNames = new HashSet<string>();
        }

        [Display(Name = "Full Name")]
        [Required(ErrorMessage = AttributesErrorMessages.RequiredErrorMessage)]
        [StringLength(AttributesConstraints.FullNameMaxLength, ErrorMessage = AttributesErrorMessages.StringLengthErrorMessage, MinimumLength = AttributesConstraints.FullNameMinLength)]
        public string FullName { get; set; }

        [Display(Name = "Biography")]
        [MaxLength(AttributesConstraints.BiographyMaxLength, ErrorMessage = AttributesErrorMessages.MaxLengthErrorMessage)]
        public string Biography { get; set; }

        [Display(Name = "Profile Photo")]
        public IFormFile ProfilePhoto { get; set; }

        [Display(Name = "Lifestyle Type")]
        [MaxLength(AttributesConstraints.UserLifestyleTypeMaxLength, ErrorMessage = AttributesErrorMessages.MaxLengthErrorMessage)]
        public string LifestyleType { get; set; }

        [Display(Name = "Allergies")]
        public IEnumerable<string> AllergenNames { get; set; }

        public UserAdditionalInfoViewModel UserAdditionalInfoViewData { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<UserAddAdditionalInfoInputModel, UserAdditionalInfoServiceModel>()
                .ForMember(
                    destination => destination.Lifestyle,
                    opts => opts.MapFrom(origin => new LifestyleServiceModel { Type = origin.LifestyleType }));
        }
    }
}
