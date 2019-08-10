namespace CookWithMe.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using CookWithMe.Data.Common.Repositories;
    using CookWithMe.Data.Models;

    using Microsoft.EntityFrameworkCore;

    public class AllergenService : IAllergenService
    {
        private readonly IRepository<Allergen> allergenRepository;

        public AllergenService(IRepository<Allergen> allergenRepository)
        {
            this.allergenRepository = allergenRepository;
        }

        public async Task<bool> CreateAllAsync(string[] allergenNames)
        {
            foreach (var allergenName in allergenNames)
            {
                var allergen = new Allergen
                {
                    Name = allergenName,
                };

                await this.allergenRepository.AddAsync(allergen);
            }

            int result = await this.allergenRepository.SaveChangesAsync();

            return result > 0;
        }

        public async Task<IEnumerable<int>> GetIdsByNamesAsync(IEnumerable<string> allergenNames)
        {
            return await this.allergenRepository
                .AllAsNoTracking()
                .Where(x => allergenNames.Contains(x.Name))
                .Select(x => x.Id)
                .ToListAsync();
        }

        public async Task<IEnumerable<string>> GetAllNamesAsync()
        {
            return await this.allergenRepository
                .AllAsNoTracking()
                .Select(x => x.Name)
                .ToListAsync();
        }

        public async Task SetAllergenToRecipeAsync(string allergenName, Recipe recipe)
        {
            recipe.Allergens.Add(new RecipeAllergen
            {
                Allergen = await this.allergenRepository.All()
                    .SingleOrDefaultAsync(x => x.Name == allergenName),
            });
        }

        public async Task SetAllergenToUserAsync(string allergenName, ApplicationUser user)
        {
            user.Allergies.Add(new UserAllergen
            {
                Allergen = await this.allergenRepository.All()
                    .SingleOrDefaultAsync(x => x.Name == allergenName),
            });
        }
    }
}
