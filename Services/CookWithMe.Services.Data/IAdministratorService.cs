namespace CookWithMe.Services.Data
{
    using System.Threading.Tasks;

    using CookWithMe.Services.Models;

    public interface IAdministratorService
    {
        Task<bool> RegisterAsync(AdministratorServiceModel model);
    }
}
