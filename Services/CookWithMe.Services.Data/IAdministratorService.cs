namespace CookWithMe.Services.Data
{
    using CookWithMe.Services.Models;
    using System.Threading.Tasks;

    public interface IAdministratorService
    {
        Task<bool> RegisterAsync(AdministratorServiceModel model);
    }
}
