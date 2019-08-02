namespace CookWithMe.Services.Data
{
    using System.Threading.Tasks;

    using CookWithMe.Services.Models;

    public interface IReviewService
    {
        Task<bool> CreateAsync(ReviewServiceModel model);
    }
}
