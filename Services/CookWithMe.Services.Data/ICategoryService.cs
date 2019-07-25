namespace CookWithMe.Services.Data
{
    using System.Threading.Tasks;

    public interface ICategoryService
    {
        Task<bool> CreateAllAsync(string[] titles);
    }
}
