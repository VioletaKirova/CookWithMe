namespace CookWithMe.Services.Data.Reviews
{
    using System.Linq;
    using System.Threading.Tasks;

    using CookWithMe.Services.Models.Reviews;

    public interface IReviewService
    {
        Task<bool> CreateAsync(ReviewServiceModel reviewServiceModel);

        Task<bool> EditAsync(string id, ReviewServiceModel reviewServiceModel);

        Task<bool> DeleteByIdAsync(string id);

        Task<ReviewServiceModel> GetByIdAsync(string id);

        IQueryable<ReviewServiceModel> GetByRecipeId(string recipeId);
    }
}
