namespace CookWithMe.Services.Data
{
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

        public async Task<bool> CreateAllAsync(string[] names)
        {
            foreach (var name in names)
            {
                var allergen = new Allergen
                {
                    Name = name,
                };

                await this.allergenRepository.AddAsync(allergen);
            }

            int result = await this.allergenRepository.SaveChangesAsync();

            return result > 0;
        }

        public IQueryable<string> GetAllNames()
        {
            return this.allergenRepository
                .AllAsNoTracking()
                .Select(x => x.Name);
        }

        public async Task SetAllergenToRecipe(string allergenName, Recipe recipe)
        {
            recipe.Allergens.Add(new RecipeAllergen
            {
                Allergen = await this.allergenRepository.All()
                    .SingleOrDefaultAsync(x => x.Name == allergenName),
            });
        }

        public async Task SetAllergenToUser(string allergenName, ApplicationUser user)
        {
            user.Allergies.Add(new UserAllergen
            {
                Allergen = await this.allergenRepository.All()
                    .SingleOrDefaultAsync(x => x.Name == allergenName),
            });
        }
    }
}
