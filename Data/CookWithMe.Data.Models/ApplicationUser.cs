namespace CookWithMe.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using CookWithMe.Data.Common.Models;

    using Microsoft.AspNetCore.Identity;

    public class ApplicationUser : IdentityUser, IAuditInfo, IDeletableEntity
    {
        public ApplicationUser()
        {
            this.Id = Guid.NewGuid().ToString();
            this.CreatedOn = DateTime.UtcNow;
            this.IsDeleted = false;
            this.Roles = new HashSet<IdentityUserRole<string>>();
            this.Claims = new HashSet<IdentityUserClaim<string>>();
            this.Logins = new HashSet<IdentityUserLogin<string>>();

            this.Allergies = new HashSet<UserAllergen>();
            this.MyRecipes = new HashSet<Recipe>();
            this.FavoriteRecipes = new HashSet<UserFavoriteRecipe>();
            this.CookedRecipes = new HashSet<UserCookedRecipe>();
            this.CookLaterRecipes = new HashSet<UserCookLaterRecipe>();
            this.Reviews = new HashSet<Review>();
        }

        // Audit info
        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        // Deletable entity
        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }

        public virtual ICollection<IdentityUserRole<string>> Roles { get; set; }

        public virtual ICollection<IdentityUserClaim<string>> Claims { get; set; }

        public virtual ICollection<IdentityUserLogin<string>> Logins { get; set; }

        // Additional info
        [Required]
        [MaxLength(50)]
        public string FullName { get; set; }

        [MaxLength(200)]
        public string Biography { get; set; }

        public string ProfilePhoto { get; set; }

        public int? LifestyleId { get; set; }

        public Lifestyle Lifestyle { get; set; }

        public ICollection<UserAllergen> Allergies { get; set; }

        public ICollection<Recipe> MyRecipes { get; set; }

        public ICollection<UserFavoriteRecipe> FavoriteRecipes { get; set; }

        public ICollection<UserCookedRecipe> CookedRecipes { get; set; }

        public ICollection<UserCookLaterRecipe> CookLaterRecipes { get; set; }

        public ICollection<Review> Reviews { get; set; }
    }
}
