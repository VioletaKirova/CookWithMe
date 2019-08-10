namespace CookWithMe.Web.ViewModels.Users.AdditionalInfo
{
    using System.Collections.Generic;

    public class UserAdditionalInfoViewModel
    {
        public IEnumerable<string> Allergies { get; set; }

        public IEnumerable<string> Lifestyles { get; set; }
    }
}
