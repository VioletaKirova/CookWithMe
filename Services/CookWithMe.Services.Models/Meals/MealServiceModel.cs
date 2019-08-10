namespace CookWithMe.Services.Models.Meals
{
    using System;
    using System.Collections.Generic;

    using CookWithMe.Data.Models;
    using CookWithMe.Services.Mapping;

    public class MealServiceModel : IMapTo<Meal>, IMapFrom<Meal>
    {
        public string Id { get; set; }

        public DateTime CreatedOn { get; set; }

        public ICollection<MealRecipeServiceModel> Recipes { get; set; }
    }
}
