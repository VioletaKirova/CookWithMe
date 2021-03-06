﻿namespace CookWithMe.Data
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

        public DbSet<NutritionalValue> NutritionalValues { get; set; }

        public DbSet<Recipe> Recipes { get; set; }

        public DbSet<RecipeAllergen> RecipeAllergens { get; set; }

        public DbSet<RecipeLifestyle> RecipeLifestyles { get; set; }

        public DbSet<Review> Reviews { get; set; }

        public DbSet<Setting> Settings { get; set; }

        public DbSet<ShoppingList> ShoppingLists { get; set; }

        public DbSet<UserAllergen> UserAllergens { get; set; }

        public DbSet<UserCookedRecipe> UserCookedRecipes { get; set; }

        public DbSet<UserFavoriteRecipe> UserFavoriteRecipes { get; set; }

        public DbSet<UserShoppingList> UserShoppingLists { get; set; }

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

            // Many-to-many unidirectional relationship between Users and ShoppingLists
            builder.Entity<UserShoppingList>()
                .HasKey(usl => new { usl.UserId, usl.ShoppingListId });

            builder.Entity<UserShoppingList>()
                .HasOne(usl => usl.User)
                .WithMany(u => u.ShoppingLists)
                .HasForeignKey(usl => usl.UserId);

            builder.Entity<UserShoppingList>()
                .HasOne(usl => usl.ShoppingList)
                .WithMany()
                .HasForeignKey(usl => usl.ShoppingListId);

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

            // Many-to-many unidirectional relationship between Recipes and Lifestyles
            builder.Entity<RecipeLifestyle>()
                .HasKey(rl => new { rl.RecipeId, rl.LifestyleId });

            builder.Entity<RecipeLifestyle>()
                .HasOne(rl => rl.Recipe)
                .WithMany(r => r.Lifestyles)
                .HasForeignKey(rl => rl.RecipeId);

            builder.Entity<RecipeLifestyle>()
                .HasOne(rl => rl.Lifestyle)
                .WithMany()
                .HasForeignKey(rl => rl.LifestyleId);

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

            // Unique constraints
            builder.Entity<Category>()
                .HasIndex(x => x.Title)
                .IsUnique();
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
