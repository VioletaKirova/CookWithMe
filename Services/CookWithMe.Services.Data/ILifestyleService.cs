namespace CookWithMe.Services.Data
{
    using System.Threading.Tasks;

    public interface ILifestyleService
    {
        Task<bool> CreateAsync(string[] types);
    }
}
