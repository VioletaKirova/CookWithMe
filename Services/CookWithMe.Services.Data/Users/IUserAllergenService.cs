namespace CookWithMe.Services.Data.Users
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using CookWithMe.Services.Models.Users;

    public interface IUserAllergenService
    {
        Task<ICollection<UserAllergenServiceModel>> GetByUserIdAsync(string userId);

        void DeletePreviousUserAllergensByUserId(string userId);
    }
}
