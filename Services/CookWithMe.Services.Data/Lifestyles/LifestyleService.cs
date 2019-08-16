namespace CookWithMe.Services.Data.Lifestyles
{
    using System;
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
        private const string InvalidLifestyleTypeErrorMessage = "Category with Type: {0} does not exist.";

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
            var lifestyle = await this.lifestyleRepository.All()
                    .SingleOrDefaultAsync(x => x.Type == lifestyleType);

            if (lifestyle == null)
            {
                throw new ArgumentNullException(string.Format(InvalidLifestyleTypeErrorMessage, lifestyleType));
            }

            recipe.Lifestyles.Add(new RecipeLifestyle
            {
                Lifestyle = lifestyle,
            });
        }

        public async Task SetLifestyleToUserAsync(string lifestyleType, ApplicationUser user)
        {
            var lifestyle = await this.lifestyleRepository.All()
                .SingleOrDefaultAsync(x => x.Type == lifestyleType);

            if (lifestyle == null)
            {
                throw new ArgumentNullException(string.Format(InvalidLifestyleTypeErrorMessage, lifestyleType));
            }

            user.Lifestyle = lifestyle;
        }
    }
}
