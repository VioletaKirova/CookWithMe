namespace CookWithMe.Services.Data
{
    using System.Linq;
    using System.Threading.Tasks;

    using CookWithMe.Services.Models;

    public interface IReviewService
    {
        Task<bool> CreateAsync(ReviewServiceModel serviceModel);

        Task<bool> DeleteAsync(string id);

        Task<ReviewServiceModel> GetByIdAsync(string id);

        IQueryable<ReviewServiceModel> GetAllByRecipeId(string recipeId);
    }
}
