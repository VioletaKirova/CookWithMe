namespace CookWithMe.Services.Data.Reviews
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using CookWithMe.Data.Common.Repositories;
    using CookWithMe.Data.Models;
    using CookWithMe.Services.Data.Recipes;
    using CookWithMe.Services.Data.Users;
    using CookWithMe.Services.Mapping;
    using CookWithMe.Services.Models.Reviews;

    public class ReviewService : IReviewService
    {
        private const string InvalidReviewIdErrorMessage = "Review with ID: {0} does not exist.";
        private const string InvalidReviewPropertyErrorMessage = "One or more required properties are null.";

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

            if (review.Comment == null ||
                review.Rating == 0 ||
                review.ReviewerId == null ||
                review.Recipe.Id == null)
            {
                throw new ArgumentNullException(InvalidReviewPropertyErrorMessage);
            }

            await this.userService.SetUserToReviewAsync(reviewServiceModel.ReviewerId, review);
            await this.recipeService.SetRecipeToReviewAsync(reviewServiceModel.Recipe.Id, review);

            await this.reviewRepository.AddAsync(review);
            var result = await this.reviewRepository.SaveChangesAsync();

            return result > 0;
        }

        public async Task<bool> DeleteByIdAsync(string id)
        {
            var reviewFromDb = await this.reviewRepository.GetByIdWithDeletedAsync(id);

            if (reviewFromDb == null)
            {
                throw new ArgumentNullException(
                    string.Format(InvalidReviewIdErrorMessage, id));
            }

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
            var review = await this.reviewRepository
                .GetByIdWithDeletedAsync(id);

            if (review == null)
            {
                throw new ArgumentNullException(
                    string.Format(InvalidReviewIdErrorMessage, id));
            }

            return review.To<ReviewServiceModel>();
        }

        public async Task<bool> EditAsync(string id, ReviewServiceModel reviewServiceModel)
        {
            var reviewFromDb = await this.reviewRepository.GetByIdWithDeletedAsync(id);

            if (reviewFromDb == null)
            {
                throw new ArgumentNullException(
                    string.Format(InvalidReviewIdErrorMessage, id));
            }

            if (reviewServiceModel.Comment == null ||
                reviewServiceModel.Rating == 0)
            {
                throw new ArgumentNullException(InvalidReviewPropertyErrorMessage);
            }

            reviewFromDb.Rating = reviewServiceModel.Rating;
            reviewFromDb.Comment = reviewServiceModel.Comment;

            this.reviewRepository.Update(reviewFromDb);
            var result = await this.reviewRepository.SaveChangesAsync();

            return result > 0;
        }
    }
}
