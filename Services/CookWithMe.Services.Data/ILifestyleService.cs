namespace CookWithMe.Services.Data
{
    using System.Linq;
    using System.Threading.Tasks;

    using CookWithMe.Data.Models;

    public interface ILifestyleService
    {
        Task<bool> CreateAllAsync(string[] types);

        IQueryable<string> GetAllTypes();

        Task SetLifestyleToRecipe(string lifestyleType, Recipe recipe);

        Task SetLifestyleToUser(string lifestyleType, ApplicationUser user);
    }
}
