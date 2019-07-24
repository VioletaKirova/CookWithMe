namespace CookWithMe.Services.Data
{
    using System.Threading.Tasks;

    using CookWithMe.Data.Common.Repositories;
    using CookWithMe.Data.Models;

    public class LifestyleService : ILifestyleService
    {
        private readonly IRepository<Lifestyle> lifestyleRepository;

        public LifestyleService(IRepository<Lifestyle> lifestyleRepository)
        {
            this.lifestyleRepository = lifestyleRepository;
        }

        public async Task<bool> CreateAsync(string[] types)
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
    }
}
