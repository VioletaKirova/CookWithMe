﻿namespace CookWithMe.Services.Data.Allergens
{
    using System;
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
            var allergenIds = await this.allergenRepository
                .AllAsNoTracking()
                .Where(x => allergenNames.Contains(x.Name))
                .Select(x => x.Id)
                .ToListAsync();

            if (allergenIds.Count != allergenNames.Count())
            {
                throw new ArgumentNullException();
            }

            return allergenIds;
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
            var allergen = await this.allergenRepository.All()
                .SingleOrDefaultAsync(x => x.Name == allergenName);

            if (allergen == null)
            {
                throw new ArgumentNullException();
            }

            recipe.Allergens.Add(new RecipeAllergen
            {
                Allergen = allergen,
            });
        }

        public async Task SetAllergenToUserAsync(string allergenName, ApplicationUser user)
        {
            var allergen = await this.allergenRepository.All()
                    .SingleOrDefaultAsync(x => x.Name == allergenName);

            if (allergen == null)
            {
                throw new ArgumentNullException();
            }

            user.Allergies.Add(new UserAllergen
            {
                Allergen = allergen,
            });
        }
    }
}
