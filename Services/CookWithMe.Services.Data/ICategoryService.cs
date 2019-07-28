namespace CookWithMe.Services.Data
{
    using System.Linq;
    using System.Threading.Tasks;

    using CookWithMe.Services.Models;

    public interface ICategoryService
    {
        Task<bool> CreateAllAsync(string[] titles);

        IQueryable<string> GetAllTitles();

        Task<bool> CreateAsync(CategoryServiceModel model);

        Task<int> GetIdByTitle(string title);
    }
}
