namespace CookWithMe.Services.Models
{
    using System.Collections.Generic;

    public class UserAdditionalInfoServiceModel
    {
        public UserAdditionalInfoServiceModel()
        {
            this.Allergies = new HashSet<UserAllergenServiceModel>();
        }

        public string Biography { get; set; }

        public string ProfilePhoto { get; set; }

        public LifestyleServiceModel Lifestyle { get; set; }

        public ICollection<UserAllergenServiceModel> Allergies { get; set; }
    }
}
