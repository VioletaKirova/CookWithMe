namespace CookWithMe.Services.Data
{
    using System.Threading.Tasks;

    using CookWithMe.Services.Models;

    public interface IUserService
    {
        Task<bool> UpdateUserAdditionalInfoAsync(string userId, UserAdditionalInfoServiceModel additionalInfoServiceModel);
    }
}
