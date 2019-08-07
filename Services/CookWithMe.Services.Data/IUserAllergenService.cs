namespace CookWithMe.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using CookWithMe.Services.Models;

    public interface IUserAllergenService
    {
        Task<List<UserAllergenServiceModel>> GetByUserId(string userId);

        void DeletePreviousAllergensByUserId(string userId);
    }
}
