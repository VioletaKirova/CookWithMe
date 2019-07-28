namespace CookWithMe.Services.Models
{
    using System.Collections.Generic;

    public class UserAdditionalInfoServiceModel
    {
        public UserAdditionalInfoServiceModel()
        {
            this.Allergies = new HashSet<string>();
        }

        public string Biography { get; set; }

        public string ProfilePhoto { get; set; }

        public string LifestyleType { get; set; }

        public IEnumerable<string> Allergies { get; set; }
    }
}
