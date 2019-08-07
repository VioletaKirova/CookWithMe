namespace CookWithMe.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using CookWithMe.Data.Common.Repositories;
    using CookWithMe.Data.Models;
    using CookWithMe.Services.Mapping;
    using CookWithMe.Services.Models;

    using Microsoft.EntityFrameworkCore;

    public class UserAllergenService : IUserAllergenService
    {
        private readonly IRepository<UserAllergen> userAllergenRepository;

        public UserAllergenService(IRepository<UserAllergen> userAllergenRepository)
        {
            this.userAllergenRepository = userAllergenRepository;
        }

        public void DeletePreviousAllergensByUserId(string userId)
        {
            var userAllergens = this.userAllergenRepository.All().Where(x => x.UserId == userId);

            foreach (var userAllergen in userAllergens)
            {
                this.userAllergenRepository.Delete(userAllergen);
            }
        }

        public async Task<List<UserAllergenServiceModel>> GetByUserId(string userId)
        {
            return await this.userAllergenRepository
                .AllAsNoTracking()
                .Where(x => x.UserId == userId)
                .To<UserAllergenServiceModel>()
                .ToListAsync();
        }
    }
}
