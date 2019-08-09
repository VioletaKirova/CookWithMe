namespace CookWithMe.Web.InputModels.Users
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using AutoMapper;

    using CookWithMe.Services.Mapping;
    using CookWithMe.Services.Models;

    using Microsoft.AspNetCore.Http;

    public class UserEditAdditionalInfoInputModel : IMapTo<UserAdditionalInfoServiceModel>, IMapFrom<UserAdditionalInfoServiceModel>,  IHaveCustomMappings
    {
        public UserEditAdditionalInfoInputModel()
        {
            this.AllergenNames = new HashSet<string>();
        }

        [Required]
        [MaxLength(50)]
        public string FullName { get; set; }

        [MaxLength(200, ErrorMessage = "The {0} can be at max {1} characters long.")]
        public string Biography { get; set; }

        public IFormFile ProfilePhoto { get; set; }

        public string LifestyleType { get; set; }

        public IEnumerable<string> AllergenNames { get; set; }

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
