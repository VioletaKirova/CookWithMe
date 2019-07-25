namespace CookWithMe.Services.Data
{
    using System.Threading.Tasks;

    using CookWithMe.Data.Common.Repositories;
    using CookWithMe.Data.Models;

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
    }
}
