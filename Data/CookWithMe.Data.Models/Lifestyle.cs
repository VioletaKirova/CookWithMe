namespace CookWithMe.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using CookWithMe.Data.Common.Models;

    public class Lifestyle : BaseModel<int>
    {
        public Lifestyle()
        {
            this.CreatedOn = DateTime.UtcNow;
            this.Users = new HashSet<ApplicationUser>();
            this.Recipes = new HashSet<Recipe>();
        }

        [Required]
        [MaxLength(20)]
        public string Type { get; set; }

        public ICollection<ApplicationUser> Users { get; set; }

        public ICollection<Recipe> Recipes { get; set; }
    }
}
