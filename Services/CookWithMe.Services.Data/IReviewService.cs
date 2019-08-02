namespace CookWithMe.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using CookWithMe.Services.Models;

    public interface IReviewService
    {
        Task<bool> CreateAsync(ReviewServiceModel model);

        Task<ICollection<ReviewServiceModel>> GetAllByRecipeId(string recipeId);
    }
}
