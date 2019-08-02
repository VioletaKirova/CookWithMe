namespace CookWithMe.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using CookWithMe.Data.Common.Repositories;
    using CookWithMe.Data.Models;
    using CookWithMe.Services.Mapping;
    using CookWithMe.Services.Models;
    using Microsoft.EntityFrameworkCore;

    public class ReviewService : IReviewService
    {
        private readonly IDeletableEntityRepository<Review> reviewRepository;
        private readonly IUserService userService;
        private readonly IRecipeService recipeService;

        public ReviewService(IDeletableEntityRepository<Review> reviewRepository, IUserService userService, IRecipeService recipeService)
        {
            this.reviewRepository = reviewRepository;
            this.userService = userService;
            this.recipeService = recipeService;
        }

        public async Task<bool> CreateAsync(ReviewServiceModel model)
        {
            var review = model.To<Review>();

            review.Id = Guid.NewGuid().ToString();

            await this.reviewRepository.AddAsync(review);
            await this.reviewRepository.SaveChangesAsync();

            await this.userService.SetUserToReview(model.ReviewerId, review);
            await this.recipeService.SetRecipeToReview(model.RecipeId, review);

            this.reviewRepository.Update(review);
            var result = await this.reviewRepository.SaveChangesAsync();

            return result > 0;
        }

        public async Task<ICollection<ReviewServiceModel>> GetAllByRecipeId(string recipeId)
        {
            return await this.reviewRepository
                .AllAsNoTracking()
                .Where(x => x.RecipeId == recipeId)
                .To<ReviewServiceModel>()
                .ToListAsync();
        }
    }
}
