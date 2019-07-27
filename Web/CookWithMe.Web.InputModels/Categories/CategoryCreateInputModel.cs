namespace CookWithMe.Web.InputModels.Categories
{
    using CookWithMe.Services.Mapping;
    using CookWithMe.Services.Models;

    public class CategoryCreateInputModel : IMapTo<CategoryServiceModel>
    {
        public string Title { get; set; }
    }
}
