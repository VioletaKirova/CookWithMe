namespace CookWithMe.Services.Data
{
    using System.Threading.Tasks;

    using CookWithMe.Services.Models;

    public interface ICategoryService
    {
        Task<bool> CreateAllAsync(string[] titles);

        Task<bool> CreateAsync(CategoryServiceModel model);
    }
}
