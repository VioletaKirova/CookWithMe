﻿namespace CookWithMe.Web.Areas.Identity.Pages.Account.Manage
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Threading.Tasks;
    using CookWithMe.Common;
    using CookWithMe.Data.Common.Repositories;
    using CookWithMe.Data.Models;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;

#pragma warning disable SA1649 // File name should match first type name
    public class DeletePersonalDataModel : PageModel
#pragma warning restore SA1649 // File name should match first type name
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly ILogger<DeletePersonalDataModel> logger;
        private readonly IDeletableEntityRepository<ApplicationUser> userRepository;
        private readonly IDeletableEntityRepository<Review> reviewRepository;
        private readonly IRepository<UserAllergen> userAllergenRepository;
        private readonly IRepository<UserShoppingList> userShoppingListRepository;
        private readonly IRepository<UserFavoriteRecipe> userFavoriteRecipeRepository;
        private readonly IRepository<UserCookedRecipe> userCookedRecipeRepository;

        public DeletePersonalDataModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<DeletePersonalDataModel> logger,
            IDeletableEntityRepository<ApplicationUser> userRepository,
            IDeletableEntityRepository<Review> reviewRepository,
            IRepository<UserAllergen> userAllergenRepository,
            IRepository<UserShoppingList> userShoppingListRepository,
            IRepository<UserFavoriteRecipe> userFavoriteRecipeRepository,
            IRepository<UserCookedRecipe> userCookedRecipeRepository)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.logger = logger;
            this.userRepository = userRepository;
            this.reviewRepository = reviewRepository;
            this.userAllergenRepository = userAllergenRepository;
            this.userShoppingListRepository = userShoppingListRepository;
            this.userFavoriteRecipeRepository = userFavoriteRecipeRepository;
            this.userCookedRecipeRepository = userCookedRecipeRepository;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public bool RequirePassword { get; set; }

        public async Task<IActionResult> OnGet()
        {
            var user = await this.userManager.GetUserAsync(this.User);
            if (user == null)
            {
                return this.NotFound($"Unable to load user with ID '{this.userManager.GetUserId(this.User)}'.");
            }

            this.RequirePassword = await this.userManager.HasPasswordAsync(user);
            return this.Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await this.userManager.GetUserAsync(this.User);
            if (user == null)
            {
                return this.NotFound($"Unable to load user with ID '{this.userManager.GetUserId(this.User)}'.");
            }

            this.RequirePassword = await this.userManager.HasPasswordAsync(user);
            if (this.RequirePassword)
            {
                if (!await this.userManager.CheckPasswordAsync(user, this.Input.Password))
                {
                    this.ModelState.AddModelError(string.Empty, "Password not correct.");
                    return this.Page();
                }
            }

            var userId = await this.userManager.GetUserIdAsync(user);

            if (this.User.IsInRole(GlobalConstants.AdministratorRoleName))
            {
                this.userRepository.Delete(user);
            }
            else
            {
                // Delete all Reviews
                var userReviews = await this.reviewRepository
                    .All()
                    .Where(x => x.ReviewerId == userId)
                    .ToListAsync();

                foreach (var review in userReviews)
                {
                    this.reviewRepository.HardDelete(review);
                }

                await this.reviewRepository.SaveChangesAsync();

                // Delete all UserAllergens
                var userAllergens = await this.userAllergenRepository
                    .All()
                    .Where(x => x.UserId == userId)
                    .ToListAsync();

                foreach (var userAllergen in userAllergens)
                {
                    this.userAllergenRepository.Delete(userAllergen);
                }

                await this.userAllergenRepository.SaveChangesAsync();

                // Delete all UserShoppinglists
                var userShoppingLists = await this.userShoppingListRepository
                    .All()
                    .Where(x => x.UserId == userId)
                    .ToListAsync();

                foreach (var userShoppingList in userShoppingLists)
                {
                    this.userShoppingListRepository.Delete(userShoppingList);
                }

                await this.userShoppingListRepository.SaveChangesAsync();

                // Delete all UserFavoriteRecipes
                var userFavoriteRecipes = await this.userFavoriteRecipeRepository
                    .All()
                    .Where(x => x.UserId == userId)
                    .ToListAsync();

                foreach (var userFavoriteRecipe in userFavoriteRecipes)
                {
                    this.userFavoriteRecipeRepository.Delete(userFavoriteRecipe);
                }

                await this.userFavoriteRecipeRepository.SaveChangesAsync();

                // Delete all UserCookedRecipes
                var userCookedRecipes = await this.userCookedRecipeRepository
                    .All()
                    .Where(x => x.UserId == userId)
                    .ToListAsync();

                foreach (var userCookedRecipe in userCookedRecipes)
                {
                    this.userCookedRecipeRepository.Delete(userCookedRecipe);
                }

                await this.userCookedRecipeRepository.SaveChangesAsync();

                // Delete personal data
                user.UserName = null;
                user.NormalizedUserName = null;
                user.Email = null;
                user.NormalizedEmail = null;
                user.PasswordHash = null;
                user.FullName = "DELETED";
                user.Biography = null;
                user.ProfilePhoto = null;
                user.LifestyleId = null;
                user.HasAdditionalInfo = false;

                this.userRepository.Update(user);
                await this.userRepository.SaveChangesAsync();

                this.userRepository.Delete(user);
            }

            var result = await this.userRepository.SaveChangesAsync();

            if (result > 0 == false)
            {
                throw new InvalidOperationException($"Unexpected error occurred deleting user with ID '{userId}'.");
            }

            await this.signInManager.SignOutAsync();

            this.logger.LogInformation("User with ID '{UserId}' deleted themselves.", userId);

            return this.Redirect("~/");
        }

        public class InputModel
        {
            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }
        }
    }
}
