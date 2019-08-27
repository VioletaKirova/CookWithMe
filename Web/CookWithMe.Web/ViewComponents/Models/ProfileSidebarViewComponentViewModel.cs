namespace CookWithMe.Web.ViewComponents.Models
{
    using CookWithMe.Services.Mapping;
    using CookWithMe.Services.Models.Users;

    public class ProfileSidebarViewComponentViewModel : IMapFrom<ApplicationUserServiceModel>
    {
        public string Id { get; set; }

        public string FullName { get; set; }

        public string Biography { get; set; }

        public string ProfilePhoto { get; set; }

        public string LifestyleType { get; set; }
    }
}
