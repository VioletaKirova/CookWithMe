namespace CookWithMe.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IAllergenService
    {
        Task<bool> CreateAllAsync(string[] names);

        Task<IEnumerable<string>> GetAllNamesAsync();

        Task<int> GetIdByName(string name);
    }
}
