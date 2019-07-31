﻿namespace CookWithMe.Services.Models
{
    using CookWithMe.Data.Models;
    using CookWithMe.Services.Mapping;

    public class AllergenServiceModel : IMapTo<Allergen>, IMapFrom<Allergen>
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
