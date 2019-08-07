namespace CookWithMe.Web.ViewModels.Categories.Delete
{
    using CookWithMe.Services.Mapping;
    using CookWithMe.Services.Models;

    public class CategoryDeleteViewModel : IMapTo<CategoryServiceModel>, IMapFrom<CategoryServiceModel>
    {
        public int Id { get; set; }


        public string Title { get; set; }
    }
}