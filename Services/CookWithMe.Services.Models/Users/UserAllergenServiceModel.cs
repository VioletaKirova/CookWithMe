namespace CookWithMe.Services.Models.Users
{
    using CookWithMe.Data.Models;
    using CookWithMe.Services.Mapping;
    using CookWithMe.Services.Models.Allergens;

    public class UserAllergenServiceModel : IMapTo<UserAllergen>, IMapFrom<UserAllergen>
    {
        public string UserId { get; set; }

        public ApplicationUserServiceModel User { get; set; }

        public int AllergenId { get; set; }

        public AllergenServiceModel Allergen { get; set; }
    }
}
