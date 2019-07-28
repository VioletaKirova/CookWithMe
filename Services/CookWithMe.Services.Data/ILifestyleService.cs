namespace CookWithMe.Services.Data
{
    using System.Linq;
    using System.Threading.Tasks;

    public interface ILifestyleService
    {
        Task<bool> CreateAllAsync(string[] types);

        IQueryable<string> GetAllTypes();
    }
}
