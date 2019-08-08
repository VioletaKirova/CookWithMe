namespace CookWithMe.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using CookWithMe.Services.Models;

    public interface IUserAllergenService
    {
        Task<ICollection<UserAllergenServiceModel>> GetByUserId(string userId);

        void DeletePreviousUserAllergensByUserId(string userId);
    }
}
