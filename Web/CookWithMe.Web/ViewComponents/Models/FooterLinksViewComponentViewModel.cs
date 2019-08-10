namespace CookWithMe.Web.ViewComponents.Models
{
    using CookWithMe.Services.Mapping;
    using CookWithMe.Services.Models.Categories;

    public class FooterLinksViewComponentViewModel : IMapFrom<CategoryServiceModel>
    {
        public int Id { get; set; }

        public string Title { get; set; }
    }
}
