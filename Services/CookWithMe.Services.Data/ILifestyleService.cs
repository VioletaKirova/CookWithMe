namespace CookWithMe.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public interface ILifestyleService
    {
        Task<bool> CreateAllAsync(string[] types);

        IQueryable<string> GetAllTypes();

        Task<int> GetIdByType(string type);
    }
}
