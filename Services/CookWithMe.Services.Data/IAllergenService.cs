namespace CookWithMe.Services.Data
{
    using System.Linq;
    using System.Threading.Tasks;

    public interface IAllergenService
    {
        Task<bool> CreateAllAsync(string[] names);

        IQueryable<string> GetAllNames();

        Task<int> GetIdByName(string name);
    }
}
