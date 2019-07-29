namespace CookWithMe.Services.Data
{
    using System.Linq;
    using System.Threading.Tasks;

    using CookWithMe.Data.Common.Repositories;
    using CookWithMe.Data.Models;

    using Microsoft.EntityFrameworkCore;

    public class LifestyleService : ILifestyleService
    {
        private readonly IRepository<Lifestyle> lifestyleRepository;

        public LifestyleService(IRepository<Lifestyle> lifestyleRepository)
        {
            this.lifestyleRepository = lifestyleRepository;
        }

        public async Task<bool> CreateAllAsync(string[] types)
        {
            foreach (var type in types)
            {
                var lifestyle = new Lifestyle
                {
                    Type = type,
                };

                await this.lifestyleRepository.AddAsync(lifestyle);
            }

            var result = await this.lifestyleRepository.SaveChangesAsync();

            return result > 0;
        }

        public IQueryable<string> GetAllTypes()
        {
            return this.lifestyleRepository
                .AllAsNoTracking()
                .Select(x => x.Type);
        }

        public async Task SetLifestyleToRecipe(string lifestyleType, Recipe recipe)
        {
            recipe.Lifestyle = await this.lifestyleRepository.All()
                .SingleOrDefaultAsync(x => x.Type == lifestyleType);
        }

        public async Task SetLifestyleToUser(string lifestyleType, ApplicationUser user)
        {
            user.Lifestyle = await this.lifestyleRepository.All()
                .SingleOrDefaultAsync(x => x.Type == lifestyleType);
        }
    }
}
