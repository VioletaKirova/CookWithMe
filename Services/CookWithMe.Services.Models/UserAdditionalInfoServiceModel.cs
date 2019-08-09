namespace CookWithMe.Services.Models
{
    using System.Collections.Generic;

    using CookWithMe.Data.Models;
    using CookWithMe.Services.Mapping;

    public class UserAdditionalInfoServiceModel : IMapFrom<ApplicationUser>
    {
        public UserAdditionalInfoServiceModel()
        {
            this.Allergies = new HashSet<UserAllergenServiceModel>();
        }

        public string FullName { get; set; }

        public string Biography { get; set; }

        public string ProfilePhoto { get; set; }

        public int? LifestyleId { get; set; }

        public LifestyleServiceModel Lifestyle { get; set; }

        public ICollection<UserAllergenServiceModel> Allergies { get; set; }
    }
}
