namespace CookWithMe.Services.Models.Categories
{
    using CookWithMe.Data.Models;
    using CookWithMe.Services.Mapping;

    public class CategoryServiceModel : IMapTo<Category>, IMapFrom<Category>
    {
        public int Id { get; set; }

        public string Title { get; set; }
    }
}
