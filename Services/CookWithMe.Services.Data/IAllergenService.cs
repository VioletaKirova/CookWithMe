namespace CookWithMe.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using CookWithMe.Data.Models;

    public interface IAllergenService
    {
        Task<bool> CreateAllAsync(string[] names);

        IQueryable<string> GetAllNames();

        Task SetAllergenToRecipe(string allergenName, Recipe recipe);

        Task SetAllergenToUser(string allergenName, ApplicationUser user);

        Task<IEnumerable<int>> GetAllIds(IEnumerable<string> allergenNames);
    }
}
