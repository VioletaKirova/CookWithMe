﻿// <auto-generated />
using System;
using CookWithMe.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace CookWithMe.Data.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20190824101017_ChangingRecipeSummaryMaxValue")]
    partial class ChangingRecipeSummaryMaxValue
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.4-servicing-10062")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("CookWithMe.Data.Models.Allergen", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreatedOn");

                    b.Property<DateTime?>("ModifiedOn");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(20);

                    b.HasKey("Id");

                    b.ToTable("Allergens");
                });

            modelBuilder.Entity("CookWithMe.Data.Models.ApplicationRole", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<DateTime>("CreatedOn");

                    b.Property<DateTime?>("DeletedOn");

                    b.Property<bool>("IsDeleted");

                    b.Property<DateTime?>("ModifiedOn");

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("IsDeleted");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("CookWithMe.Data.Models.ApplicationUser", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("Biography")
                        .HasMaxLength(200);

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<DateTime>("CreatedOn");

                    b.Property<DateTime?>("DeletedOn");

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<bool>("HasAdditionalInfo");

                    b.Property<bool>("IsDeleted");

                    b.Property<int?>("LifestyleId");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<DateTime?>("ModifiedOn");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("ProfilePhoto");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("IsDeleted");

                    b.HasIndex("LifestyleId");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("CookWithMe.Data.Models.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreatedOn");

                    b.Property<DateTime?>("DeletedOn");

                    b.Property<bool>("IsDeleted");

                    b.Property<DateTime?>("ModifiedOn");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(20);

                    b.HasKey("Id");

                    b.HasIndex("IsDeleted");

                    b.HasIndex("Title")
                        .IsUnique();

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("CookWithMe.Data.Models.Lifestyle", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreatedOn");

                    b.Property<DateTime?>("ModifiedOn");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasMaxLength(20);

                    b.HasKey("Id");

                    b.ToTable("Lifestyles");
                });

            modelBuilder.Entity("CookWithMe.Data.Models.NutritionalValue", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<double?>("Calories");

                    b.Property<double?>("Carbohydrates");

                    b.Property<DateTime>("CreatedOn");

                    b.Property<DateTime?>("DeletedOn");

                    b.Property<double?>("Fats");

                    b.Property<double?>("Fiber");

                    b.Property<bool>("IsDeleted");

                    b.Property<DateTime?>("ModifiedOn");

                    b.Property<double?>("Protein");

                    b.Property<string>("RecipeId")
                        .IsRequired();

                    b.Property<double?>("Salt");

                    b.Property<double?>("SaturatedFats");

                    b.Property<double?>("Sugar");

                    b.HasKey("Id");

                    b.HasIndex("IsDeleted");

                    b.HasIndex("RecipeId")
                        .IsUnique();

                    b.ToTable("NutritionalValues");
                });

            modelBuilder.Entity("CookWithMe.Data.Models.Recipe", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("CategoryId");

                    b.Property<int>("CookingTime");

                    b.Property<DateTime>("CreatedOn");

                    b.Property<DateTime?>("DeletedOn");

                    b.Property<string>("Directions")
                        .IsRequired();

                    b.Property<bool>("IsDeleted");

                    b.Property<DateTime?>("ModifiedOn");

                    b.Property<int>("NeededTime");

                    b.Property<string>("NutritionalValueId");

                    b.Property<string>("Photo")
                        .IsRequired();

                    b.Property<int>("PreparationTime");

                    b.Property<int>("Serving");

                    b.Property<string>("ShoppingListId");

                    b.Property<int>("SkillLevel");

                    b.Property<string>("Summary")
                        .IsRequired()
                        .HasMaxLength(1000);

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.Property<decimal?>("Yield");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.HasIndex("IsDeleted");

                    b.HasIndex("UserId");

                    b.ToTable("Recipes");
                });

            modelBuilder.Entity("CookWithMe.Data.Models.RecipeAllergen", b =>
                {
                    b.Property<string>("RecipeId");

                    b.Property<int>("AllergenId");

                    b.HasKey("RecipeId", "AllergenId");

                    b.HasIndex("AllergenId");

                    b.ToTable("RecipeAllergens");
                });

            modelBuilder.Entity("CookWithMe.Data.Models.RecipeLifestyle", b =>
                {
                    b.Property<string>("RecipeId");

                    b.Property<int>("LifestyleId");

                    b.HasKey("RecipeId", "LifestyleId");

                    b.HasIndex("LifestyleId");

                    b.ToTable("RecipeLifestyles");
                });

            modelBuilder.Entity("CookWithMe.Data.Models.Review", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Comment")
                        .IsRequired()
                        .HasMaxLength(250);

                    b.Property<DateTime>("CreatedOn");

                    b.Property<DateTime?>("DeletedOn");

                    b.Property<bool>("IsDeleted");

                    b.Property<DateTime?>("ModifiedOn");

                    b.Property<int>("Rating");

                    b.Property<string>("RecipeId")
                        .IsRequired();

                    b.Property<string>("ReviewerId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("IsDeleted");

                    b.HasIndex("RecipeId");

                    b.HasIndex("ReviewerId");

                    b.ToTable("Reviews");
                });

            modelBuilder.Entity("CookWithMe.Data.Models.Setting", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreatedOn");

                    b.Property<DateTime?>("DeletedOn");

                    b.Property<bool>("IsDeleted");

                    b.Property<DateTime?>("ModifiedOn");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("Id");

                    b.HasIndex("IsDeleted");

                    b.ToTable("Settings");
                });

            modelBuilder.Entity("CookWithMe.Data.Models.ShoppingList", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedOn");

                    b.Property<DateTime?>("DeletedOn");

                    b.Property<string>("Ingredients")
                        .IsRequired();

                    b.Property<bool>("IsDeleted");

                    b.Property<DateTime?>("ModifiedOn");

                    b.Property<string>("RecipeId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("IsDeleted");

                    b.HasIndex("RecipeId")
                        .IsUnique();

                    b.ToTable("ShoppingLists");
                });

            modelBuilder.Entity("CookWithMe.Data.Models.UserAllergen", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<int>("AllergenId");

                    b.HasKey("UserId", "AllergenId");

                    b.HasIndex("AllergenId");

                    b.ToTable("UserAllergens");
                });

            modelBuilder.Entity("CookWithMe.Data.Models.UserCookedRecipe", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RecipeId");

                    b.Property<DateTime>("AddedOn");

                    b.HasKey("UserId", "RecipeId");

                    b.HasIndex("RecipeId");

                    b.ToTable("UserCookedRecipes");
                });

            modelBuilder.Entity("CookWithMe.Data.Models.UserFavoriteRecipe", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RecipeId");

                    b.Property<DateTime>("AddedOn");

                    b.HasKey("UserId", "RecipeId");

                    b.HasIndex("RecipeId");

                    b.ToTable("UserFavoriteRecipes");
                });

            modelBuilder.Entity("CookWithMe.Data.Models.UserShoppingList", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("ShoppingListId");

                    b.Property<DateTime>("AddedOn");

                    b.HasKey("UserId", "ShoppingListId");

                    b.HasIndex("ShoppingListId");

                    b.ToTable("UserShoppingLists");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("CookWithMe.Data.Models.ApplicationUser", b =>
                {
                    b.HasOne("CookWithMe.Data.Models.Lifestyle", "Lifestyle")
                        .WithMany("Users")
                        .HasForeignKey("LifestyleId");
                });

            modelBuilder.Entity("CookWithMe.Data.Models.NutritionalValue", b =>
                {
                    b.HasOne("CookWithMe.Data.Models.Recipe", "Recipe")
                        .WithOne("NutritionalValue")
                        .HasForeignKey("CookWithMe.Data.Models.NutritionalValue", "RecipeId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("CookWithMe.Data.Models.Recipe", b =>
                {
                    b.HasOne("CookWithMe.Data.Models.Category", "Category")
                        .WithMany("Recipes")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("CookWithMe.Data.Models.ApplicationUser", "User")
                        .WithMany("Recipes")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("CookWithMe.Data.Models.RecipeAllergen", b =>
                {
                    b.HasOne("CookWithMe.Data.Models.Allergen", "Allergen")
                        .WithMany()
                        .HasForeignKey("AllergenId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("CookWithMe.Data.Models.Recipe", "Recipe")
                        .WithMany("Allergens")
                        .HasForeignKey("RecipeId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("CookWithMe.Data.Models.RecipeLifestyle", b =>
                {
                    b.HasOne("CookWithMe.Data.Models.Lifestyle", "Lifestyle")
                        .WithMany()
                        .HasForeignKey("LifestyleId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("CookWithMe.Data.Models.Recipe", "Recipe")
                        .WithMany("Lifestyles")
                        .HasForeignKey("RecipeId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("CookWithMe.Data.Models.Review", b =>
                {
                    b.HasOne("CookWithMe.Data.Models.Recipe", "Recipe")
                        .WithMany("Reviews")
                        .HasForeignKey("RecipeId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("CookWithMe.Data.Models.ApplicationUser", "Reviewer")
                        .WithMany("Reviews")
                        .HasForeignKey("ReviewerId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("CookWithMe.Data.Models.ShoppingList", b =>
                {
                    b.HasOne("CookWithMe.Data.Models.Recipe", "Recipe")
                        .WithOne("ShoppingList")
                        .HasForeignKey("CookWithMe.Data.Models.ShoppingList", "RecipeId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("CookWithMe.Data.Models.UserAllergen", b =>
                {
                    b.HasOne("CookWithMe.Data.Models.Allergen", "Allergen")
                        .WithMany()
                        .HasForeignKey("AllergenId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("CookWithMe.Data.Models.ApplicationUser", "User")
                        .WithMany("Allergies")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("CookWithMe.Data.Models.UserCookedRecipe", b =>
                {
                    b.HasOne("CookWithMe.Data.Models.Recipe", "Recipe")
                        .WithMany("CookedBy")
                        .HasForeignKey("RecipeId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("CookWithMe.Data.Models.ApplicationUser", "User")
                        .WithMany("CookedRecipes")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("CookWithMe.Data.Models.UserFavoriteRecipe", b =>
                {
                    b.HasOne("CookWithMe.Data.Models.Recipe", "Recipe")
                        .WithMany("FavoritedBy")
                        .HasForeignKey("RecipeId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("CookWithMe.Data.Models.ApplicationUser", "User")
                        .WithMany("FavoriteRecipes")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("CookWithMe.Data.Models.UserShoppingList", b =>
                {
                    b.HasOne("CookWithMe.Data.Models.ShoppingList", "ShoppingList")
                        .WithMany()
                        .HasForeignKey("ShoppingListId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("CookWithMe.Data.Models.ApplicationUser", "User")
                        .WithMany("ShoppingLists")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("CookWithMe.Data.Models.ApplicationRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("CookWithMe.Data.Models.ApplicationUser")
                        .WithMany("Claims")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("CookWithMe.Data.Models.ApplicationUser")
                        .WithMany("Logins")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("CookWithMe.Data.Models.ApplicationRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("CookWithMe.Data.Models.ApplicationUser")
                        .WithMany("Roles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("CookWithMe.Data.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict);
                });
#pragma warning restore 612, 618
        }
    }
}
