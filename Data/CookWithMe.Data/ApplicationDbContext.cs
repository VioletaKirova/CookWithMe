namespace CookWithMe.Data
{
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Threading;
    using System.Threading.Tasks;

    using CookWithMe.Data.Common.Models;
    using CookWithMe.Data.Models;

    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
    {
        private static readonly MethodInfo SetIsDeletedQueryFilterMethod =
            typeof(ApplicationDbContext).GetMethod(
                nameof(SetIsDeletedQueryFilter),
                BindingFlags.NonPublic | BindingFlags.Static);

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Allergen> Allergens { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Lifestyle> Lifestyles { get; set; }

        public DbSet<Meal> Meals { get; set; }

        public DbSet<MealRecipe> MealRecipes { get; set; }

        public DbSet<NutritionalValue> NutritionalValues { get; set; }

        public DbSet<Recipe> Recipes { get; set; }

        public DbSet<RecipeAllergen> RecipeAllergens { get; set; }

        public DbSet<Review> Reviews { get; set; }

        public DbSet<Setting> Settings { get; set; }

        public DbSet<ShoppingList> ShoppingLists { get; set; }

        public DbSet<UserAllergen> UserAllergens { get; set; }

        public DbSet<UserCookedRecipe> UserCookedRecipes { get; set; }

        public DbSet<UserCookLaterRecipe> UserCookLaterRecipes { get; set; }

        public DbSet<UserFavoriteRecipe> UserFavoriteRecipes { get; set; }

        public override int SaveChanges() => this.SaveChanges(true);

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            this.ApplyAuditInfoRules();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) =>
            this.SaveChangesAsync(true, cancellationToken);

        public override Task<int> SaveChangesAsync(
            bool acceptAllChangesOnSuccess,
            CancellationToken cancellationToken = default)
        {
            this.ApplyAuditInfoRules();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // Needed for Identity models configuration
            base.OnModelCreating(builder);

            ConfigureUserIdentityRelations(builder);

            EntityIndexesConfiguration.Configure(builder);

            var entityTypes = builder.Model.GetEntityTypes().ToList();

            // Set global query filter for not deleted entities only
            var deletableEntityTypes = entityTypes
                .Where(et => et.ClrType != null && typeof(IDeletableEntity).IsAssignableFrom(et.ClrType));
            foreach (var deletableEntityType in deletableEntityTypes)
            {
                var method = SetIsDeletedQueryFilterMethod.MakeGenericMethod(deletableEntityType.ClrType);
                method.Invoke(null, new object[] { builder });
            }

            // Disable cascade delete
            var foreignKeys = entityTypes
                .SelectMany(e => e.GetForeignKeys().Where(f => f.DeleteBehavior == DeleteBehavior.Cascade));
            foreach (var foreignKey in foreignKeys)
            {
                foreignKey.DeleteBehavior = DeleteBehavior.Restrict;
            }
        }

        private static void ConfigureUserIdentityRelations(ModelBuilder builder)
        {
            builder.Entity<ApplicationUser>()
                .HasMany(e => e.Claims)
                .WithOne()
                .HasForeignKey(e => e.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<ApplicationUser>()
                .HasMany(e => e.Logins)
                .WithOne()
                .HasForeignKey(e => e.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<ApplicationUser>()
                .HasMany(e => e.Roles)
                .WithOne()
                .HasForeignKey(e => e.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            // Many-to-many unidirectional relationship between Users and Allergens
            builder.Entity<UserAllergen>()
                .HasKey(ua => new { ua.UserId, ua.AllergenId });

            builder.Entity<UserAllergen>()
                .HasOne(ua => ua.User)
                .WithMany(u => u.Allergies)
                .HasForeignKey(ua => ua.UserId);

            builder.Entity<UserAllergen>()
                .HasOne(ua => ua.Allergen)
                .WithMany()
                .HasForeignKey(ua => ua.AllergenId);

            // Many-to-many relationship between Users and Recipes
            builder.Entity<UserFavoriteRecipe>()
                .HasKey(ufr => new { ufr.UserId, ufr.RecipeId });

            builder.Entity<UserFavoriteRecipe>()
                .HasOne(ufr => ufr.User)
                .WithMany(u => u.FavoriteRecipes)
                .HasForeignKey(ufr => ufr.UserId);

            builder.Entity<UserFavoriteRecipe>()
                .HasOne(ufr => ufr.Recipe)
                .WithMany(r => r.FavoritedBy)
                .HasForeignKey(ufr => ufr.RecipeId);

            // Many-to-many relationship between Users and Recipes
            builder.Entity<UserCookedRecipe>()
                .HasKey(ucr => new { ucr.UserId, ucr.RecipeId });

            builder.Entity<UserCookedRecipe>()
                .HasOne(ucr => ucr.User)
                .WithMany(u => u.CookedRecipes)
                .HasForeignKey(ucr => ucr.UserId);

            builder.Entity<UserCookedRecipe>()
                .HasOne(ucr => ucr.Recipe)
                .WithMany(r => r.CookedBy)
                .HasForeignKey(ucr => ucr.RecipeId);

            // Many-to-many unidirectional relationship between Users and Recipes
            builder.Entity<UserCookLaterRecipe>()
                .HasKey(uclr => new { uclr.UserId, uclr.RecipeId });

            builder.Entity<UserCookLaterRecipe>()
                .HasOne(uclr => uclr.User)
                .WithMany(u => u.CookLaterRecipes)
                .HasForeignKey(uclr => uclr.UserId);

            builder.Entity<UserCookLaterRecipe>()
                .HasOne(uclr => uclr.Recipe)
                .WithMany()
                .HasForeignKey(uclr => uclr.RecipeId);

            // Many-to-one relationship between Users and Reviews
            builder.Entity<ApplicationUser>()
                .HasMany(u => u.Reviews)
                .WithOne(r => r.Reviewer)
                .HasForeignKey(r => r.ReviewerId);

            // Many-to-one relationship between Users and Recipes
            builder.Entity<ApplicationUser>()
                .HasMany(u => u.MyRecipes)
                .WithOne(r => r.User)
                .HasForeignKey(r => r.UserId);

            // Many-to-many unidirectional relationship between Recipes and Allergens
            builder.Entity<RecipeAllergen>()
                .HasKey(ra => new { ra.RecipeId, ra.AllergenId });

            builder.Entity<RecipeAllergen>()
                .HasOne(ra => ra.Recipe)
                .WithMany(r => r.Allergens)
                .HasForeignKey(ra => ra.RecipeId);

            builder.Entity<RecipeAllergen>()
                .HasOne(ra => ra.Allergen)
                .WithMany()
                .HasForeignKey(ra => ra.AllergenId);

            // Many-to-one relationship between Recipes and Meals
            builder.Entity<MealRecipe>()
                .HasKey(mr => new { mr.MealId, mr.RecipeId });

            builder.Entity<MealRecipe>()
                .HasOne(mr => mr.Meal)
                .WithMany(m => m.Recipes)
                .HasForeignKey(mr => mr.MealId);

            builder.Entity<MealRecipe>()
                .HasOne(mr => mr.Recipe)
                .WithMany(r => r.Meals)
                .HasForeignKey(mr => mr.RecipeId);

            // Many-to-one relationship between Recipes and Reviews
            builder.Entity<Recipe>()
                .HasMany(r => r.Reviews)
                .WithOne(r => r.Recipe)
                .HasForeignKey(r => r.RecipeId);

            // One-to-one relationship between Recipes and ShoppingLists
            builder.Entity<Recipe>()
                .HasOne(r => r.ShoppingList)
                .WithOne(sl => sl.Recipe)
                .HasForeignKey<ShoppingList>(sl => sl.RecipeId);

            // One-to-one relationship between Recipes and NutritionalValues
            builder.Entity<Recipe>()
                .HasOne(r => r.NutritionalValue)
                .WithOne(nv => nv.Recipe)
                .HasForeignKey<NutritionalValue>(nv => nv.RecipeId);

            // Many-to-one relationship between Categories and Reviews
            builder.Entity<Category>()
                .HasMany(c => c.Recipes)
                .WithOne(r => r.Category)
                .HasForeignKey(r => r.CategoryId);

            // Many-to-one relationship between Lifestyles and Users
            builder.Entity<Lifestyle>()
                .HasMany(l => l.Users)
                .WithOne(u => u.Lifestyle)
                .HasForeignKey(u => u.LifestyleId);

            // Many-to-one relationship between Lifestyles and Recipes
            builder.Entity<Lifestyle>()
                .HasMany(l => l.Recipes)
                .WithOne(r => r.Lifestyle)
                .HasForeignKey(r => r.LifestyleId);
        }

        private static void SetIsDeletedQueryFilter<T>(ModelBuilder builder)
            where T : class, IDeletableEntity
        {
            builder.Entity<T>().HasQueryFilter(e => !e.IsDeleted);
        }

        private void ApplyAuditInfoRules()
        {
            var changedEntries = this.ChangeTracker
                .Entries()
                .Where(e =>
                    e.Entity is IAuditInfo &&
                    (e.State == EntityState.Added || e.State == EntityState.Modified));

            foreach (var entry in changedEntries)
            {
                var entity = (IAuditInfo)entry.Entity;
                if (entry.State == EntityState.Added && entity.CreatedOn == default)
                {
                    entity.CreatedOn = DateTime.UtcNow;
                }
                else
                {
                    entity.ModifiedOn = DateTime.UtcNow;
                }
            }
        }
    }
}
