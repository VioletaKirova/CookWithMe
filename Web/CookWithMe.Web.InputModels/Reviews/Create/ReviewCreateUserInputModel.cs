namespace CookWithMe.Web.InputModels.Reviews.Create
{
    using CookWithMe.Services.Mapping;
    using CookWithMe.Services.Models.Users;

    public class ReviewCreateUserInputModel : IMapTo<ApplicationUserServiceModel>
    {
        public string Id { get; set; }

        public string FullName { get; set; }

        public string ProfilePhoto { get; set; }
    }
}
