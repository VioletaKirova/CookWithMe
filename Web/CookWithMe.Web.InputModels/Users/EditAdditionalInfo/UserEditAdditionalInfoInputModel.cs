namespace CookWithMe.Web.InputModels.Users.EditAdditionalInfo
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

    public class UserEditAdditionalInfoInputModel : IMapTo<UserAdditionalInfoServiceModel>, IMapFrom<UserAdditionalInfoServiceModel>,  IHaveCustomMappings
    {
        public UserEditAdditionalInfoInputModel()
        {
            this.AllergenNames = new HashSet<string>();
        }

        [Display(Name = "Full Name")]
        [Required(ErrorMessage = AttributesErrorMessages.RequiredErrorMessage)]
        [StringLength(50, ErrorMessage = AttributesErrorMessages.StringLengthErrorMessage, MinimumLength = 3)]
        public string FullName { get; set; }

        [Display(Name = "Biography")]
        [MaxLength(200, ErrorMessage = AttributesErrorMessages.MaxLengthErrorMessage)]
        public string Biography { get; set; }

        [Display(Name = "Profile Photo")]
        public IFormFile ProfilePhoto { get; set; }

        [Display(Name = "Lifestyle Type")]
        [MaxLength(20, ErrorMessage = AttributesErrorMessages.MaxLengthErrorMessage)]
        public string LifestyleType { get; set; }

        [Display(Name = "Allergies")]
        [MaxLength(200, ErrorMessage = AttributesErrorMessages.MaxLengthErrorMessage)]
        public IEnumerable<string> AllergenNames { get; set; }

        public UserAdditionalInfoViewModel UserAdditionalInfoViewData { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<UserEditAdditionalInfoInputModel, UserAdditionalInfoServiceModel>()
                .ForMember(
                    destination => destination.Lifestyle,
                    opts => opts.MapFrom(origin => new LifestyleServiceModel { Type = origin.LifestyleType }));

            configuration.CreateMap<UserAdditionalInfoServiceModel, UserEditAdditionalInfoInputModel>()
                .ForMember(
                    destination => destination.ProfilePhoto,
                    opts => opts.Ignore());
        }
    }
}
