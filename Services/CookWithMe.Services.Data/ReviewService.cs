namespace CookWithMe.Services.Data
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using CookWithMe.Data.Common.Repositories;
    using CookWithMe.Data.Models;
    using CookWithMe.Services.Mapping;
    using CookWithMe.Services.Models;

    public class ReviewService : IReviewService
    {
        private readonly IDeletableEntityRepository<Review> reviewRepository;
        private readonly IUserService userService;
        private readonly IRecipeService recipeService;

        public ReviewService(
            IDeletableEntityRepository<Review> reviewRepository,
            IUserService userService,
            IRecipeService recipeService)
        {
            this.reviewRepository = reviewRepository;
            this.userService = userService;
            this.recipeService = recipeService;
        }

        public async Task<bool> CreateAsync(ReviewServiceModel reviewServiceModel)
        {
            var review = reviewServiceModel.To<Review>();

            review.Id = Guid.NewGuid().ToString();

            await this.reviewRepository.AddAsync(review);
            await this.reviewRepository.SaveChangesAsync();

            await this.userService.SetUserToReviewAsync(reviewServiceModel.ReviewerId, review);
            await this.recipeService.SetRecipeToReviewAsync(reviewServiceModel.RecipeId, review);

            this.reviewRepository.Update(review);
            var result = await this.reviewRepository.SaveChangesAsync();

            return result > 0;
        }

        public async Task<bool> DeleteByIdAsync(string id)
        {
            var reviewFromDb = await this.reviewRepository.GetByIdWithDeletedAsync(id);

            this.reviewRepository.Delete(reviewFromDb);
            var result = await this.reviewRepository.SaveChangesAsync();

            return result > 0;
        }

        public IQueryable<ReviewServiceModel> GetByRecipeId(string recipeId)
        {
            return this.reviewRepository
                .AllAsNoTracking()
                .Where(x => x.RecipeId == recipeId)
                .OrderByDescending(x => x.CreatedOn)
                .To<ReviewServiceModel>();
        }

        public async Task<ReviewServiceModel> GetByIdAsync(string id)
        {
            return (await this.reviewRepository
                .GetByIdWithDeletedAsync(id))
                .To<ReviewServiceModel>();
        }
    }
}
