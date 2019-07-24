namespace CookWithMe.Services.Data
{
    using System.Threading.Tasks;

    public interface IAllergenService
    {
        Task<bool> CreateAsync(string[] names);
    }
}
