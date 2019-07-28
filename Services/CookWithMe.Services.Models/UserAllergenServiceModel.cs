﻿namespace CookWithMe.Services.Models
{
    using CookWithMe.Data.Models;
    using CookWithMe.Services.Mapping;

    public class UserAllergenServiceModel : IMapTo<UserAllergen>
    {
        public string UserId { get; set; }

        public int AllergenId { get; set; }
    }
}
