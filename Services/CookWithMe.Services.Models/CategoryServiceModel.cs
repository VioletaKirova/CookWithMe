namespace CookWithMe.Services.Models
{
    using CookWithMe.Data.Models;
    using CookWithMe.Services.Mapping;

    public class CategoryServiceModel : IMapTo<Category>, IMapFrom<Category>
    {
        public string Id { get; set; }

        public string Title { get; set; }
    }
}
