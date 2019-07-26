namespace CookWithMe.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ILifestyleService
    {
        Task<bool> CreateAllAsync(string[] types);

        Task<IEnumerable<string>> GetAllTypesAsync();

        Task<int> GetIdByType(string type);
    }
}
