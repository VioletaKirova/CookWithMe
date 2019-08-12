namespace CookWithMe.Services.Data.Administrators
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using CookWithMe.Services.Models.Administrators;

    public interface IAdministratorService
    {
        Task<bool> RegisterAsync(AdministratorServiceModel administratorServiceModel);

        Task<IEnumerable<AdministratorServiceModel>> GetAllAsync();

        Task<bool> RemoveFromRoleByIdAsync(string userId);
    }
}
