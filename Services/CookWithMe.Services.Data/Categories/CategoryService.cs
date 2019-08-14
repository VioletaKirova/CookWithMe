namespace CookWithMe.Services.Data.Categories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using CookWithMe.Data.Common.Repositories;
    using CookWithMe.Data.Models;
    using CookWithMe.Services.Mapping;
    using CookWithMe.Services.Models.Categories;

    using Microsoft.EntityFrameworkCore;

    public class CategoryService : ICategoryService
    {
        private const string InvalidCategoryIdErrorMessage = "Category with ID: {0} does not exist.";
        private const string InvalidCategoryTitleErrorMessage = "Category with Title: {0} does not exist.";

        private readonly IDeletableEntityRepository<Category> categoryRepository;

        public CategoryService(IDeletableEntityRepository<Category> categoryRepository)
        {
            this.categoryRepository = categoryRepository;
        }

        public async Task<bool> CreateAllAsync(string[] categoryTitles)
        {
            foreach (var categoryTitle in categoryTitles)
            {
                var category = new Category
                {
                    Title = categoryTitle,
                };

                await this.categoryRepository.AddAsync(category);
            }

            var result = await this.categoryRepository.SaveChangesAsync();

            return result > 0;
        }

        public async Task<bool> CreateAsync(CategoryServiceModel categoryServiceModel)
        {
            var category = categoryServiceModel.To<Category>();

            await this.categoryRepository.AddAsync(category);
            var result = await this.categoryRepository.SaveChangesAsync();

            return result > 0;
        }

        public async Task<bool> DeleteByIdAsync(int id)
        {
            var categoryFromDb = await this.categoryRepository
                .GetByIdWithDeletedAsync(id);

            if (categoryFromDb == null)
            {
                throw new ArgumentNullException(
                    string.Format(InvalidCategoryIdErrorMessage, id));
            }

            this.categoryRepository.Delete(categoryFromDb);
            var result = await this.categoryRepository.SaveChangesAsync();

            return result > 0;
        }

        public async Task<bool> EditAsync(CategoryServiceModel categoryServiceModel)
        {
            var categoryFromDb = await this.categoryRepository
                .GetByIdWithDeletedAsync(categoryServiceModel.Id);

            if (categoryFromDb == null)
            {
                throw new ArgumentNullException(
                    string.Format(InvalidCategoryIdErrorMessage, categoryServiceModel.Id));
            }

            categoryFromDb.Title = categoryServiceModel.Title;

            this.categoryRepository.Update(categoryFromDb);
            var result = await this.categoryRepository.SaveChangesAsync();

            return result > 0;
        }

        public IQueryable<CategoryServiceModel> GetAll()
        {
            return this.categoryRepository
                .AllAsNoTracking()
                .To<CategoryServiceModel>();
        }

        public async Task<IEnumerable<string>> GetAllTitlesAsync()
        {
            return await this.categoryRepository
                .AllAsNoTracking()
                .Select(x => x.Title)
                .ToListAsync();
        }

        public async Task<CategoryServiceModel> GetByIdAsync(int id)
        {
            var category = await this.categoryRepository
                .GetByIdWithDeletedAsync(id);

            if (category == null)
            {
                throw new ArgumentNullException(
                    string.Format(InvalidCategoryIdErrorMessage, id));
            }

            return category.To<CategoryServiceModel>();
        }

        public async Task<int> GetIdByTitleAsync(string categoryTitle)
        {
            var category = await this.categoryRepository
                .AllAsNoTracking()
                .SingleOrDefaultAsync(x => x.Title == categoryTitle);

            if (category == null)
            {
                throw new ArgumentNullException(
                    string.Format(InvalidCategoryTitleErrorMessage, categoryTitle));
            }

            return category.Id;
        }

        public async Task SetCategoryToRecipeAsync(string categoryTitle, Recipe recipe)
        {
            var category = await this.categoryRepository
                .All()
                .SingleOrDefaultAsync(x => x.Title == categoryTitle);

            if (category == null)
            {
                throw new ArgumentNullException(
                    string.Format(InvalidCategoryTitleErrorMessage, categoryTitle));
            }

            recipe.Category = category;
        }
    }
}
