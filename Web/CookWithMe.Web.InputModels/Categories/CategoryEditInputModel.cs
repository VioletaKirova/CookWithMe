namespace CookWithMe.Web.InputModels.Categories
{
    using System.ComponentModel.DataAnnotations;

    using CookWithMe.Services.Mapping;
    using CookWithMe.Services.Models;

    public class CategoryEditInputModel : IMapTo<CategoryServiceModel>, IMapFrom<CategoryServiceModel>
    {
        public int Id { get; set; }

        [Required]
        [StringLength(20, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 3)]
        public string Title { get; set; }
    }
}
