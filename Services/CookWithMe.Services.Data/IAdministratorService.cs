namespace CookWithMe.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using CookWithMe.Services.Models;

    public interface IAdministratorService
    {
        Task<bool> RegisterAsync(AdministratorServiceModel administratorServiceModel);

        Task<IEnumerable<AdministratorServiceModel>> GetAllAsync();

        Task RemoveFromRoleByIdAsync(string userId);
    }
}
