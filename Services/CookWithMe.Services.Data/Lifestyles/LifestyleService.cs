namespace CookWithMe.Services.Data.Lifestyles
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using CookWithMe.Data.Common.Repositories;
    using CookWithMe.Data.Models;
    using CookWithMe.Services.Mapping;
    using CookWithMe.Services.Models.Lifestyles;

    using Microsoft.EntityFrameworkCore;

    public class LifestyleService : ILifestyleService
    {
        private readonly IRepository<Lifestyle> lifestyleRepository;

        public LifestyleService(IRepository<Lifestyle> lifestyleRepository)
        {
            this.lifestyleRepository = lifestyleRepository;
        }

        public async Task<bool> CreateAllAsync(string[] lifestyleTypes)
        {
            foreach (var lifestyleType in lifestyleTypes)
            {
                var lifestyle = new Lifestyle
                {
                    Type = lifestyleType,
                };

                await this.lifestyleRepository.AddAsync(lifestyle);
            }

            var result = await this.lifestyleRepository.SaveChangesAsync();

            return result > 0;
        }

        public async Task<IEnumerable<string>> GetAllTypesAsync()
        {
            return await this.lifestyleRepository
                .AllAsNoTracking()
                .Select(x => x.Type)
                .ToListAsync();
        }

        public async Task<LifestyleServiceModel> GetByIdAsync(int id)
        {
            return (await this.lifestyleRepository
                .AllAsNoTracking()
                .SingleOrDefaultAsync(x => x.Id == id))
                .To<LifestyleServiceModel>();
        }

        public async Task<int> GetIdByTypeAsync(string lifestyleType)
        {
            return (await this.lifestyleRepository
                .AllAsNoTracking()
                .SingleOrDefaultAsync(x => x.Type == lifestyleType))
                .Id;
        }

        public async Task SetLifestyleToRecipeAsync(string lifestyleType, Recipe recipe)
        {
            recipe.Lifestyles.Add(new RecipeLifestyle
            {
                Lifestyle = await this.lifestyleRepository.All()
                    .SingleOrDefaultAsync(x => x.Type == lifestyleType),
            });
        }

        public async Task SetLifestyleToUserAsync(string lifestyleType, ApplicationUser user)
        {
            user.Lifestyle = await this.lifestyleRepository.All()
                .SingleOrDefaultAsync(x => x.Type == lifestyleType);
        }
    }
}
