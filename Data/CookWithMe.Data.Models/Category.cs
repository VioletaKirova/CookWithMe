namespace CookWithMe.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using CookWithMe.Data.Common.Models;

    public class Category : BaseDeletableModel<int>
    {
        public Category()
        {
            this.CreatedOn = DateTime.UtcNow;
            this.IsDeleted = false;
            this.Recipes = new HashSet<Recipe>();
        }

        [Required]
        [MaxLength(20)]
        public string Title { get; set; }

        public ICollection<Recipe> Recipes { get; set; }
    }
}
