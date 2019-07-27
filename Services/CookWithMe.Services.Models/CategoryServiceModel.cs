namespace CookWithMe.Services.Models
{
    using CookWithMe.Data.Models;
    using CookWithMe.Services.Mapping;

    public class CategoryServiceModel : IMapTo<Category>
    {
        public string Title { get; set; }
    }
}
