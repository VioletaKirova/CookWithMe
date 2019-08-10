namespace CookWithMe.Services.Data
{
    using System.Linq;
    using System.Threading.Tasks;

    using CookWithMe.Services.Models;

    public interface IReviewService
    {
        Task<bool> CreateAsync(ReviewServiceModel reviewServiceModel);

        Task<bool> DeleteByIdAsync(string id);

        Task<ReviewServiceModel> GetByIdAsync(string id);

        IQueryable<ReviewServiceModel> GetByRecipeId(string recipeId);
    }
}
