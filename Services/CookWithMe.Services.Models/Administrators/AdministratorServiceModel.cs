namespace CookWithMe.Services.Models.Administrators
{
    using CookWithMe.Data.Models;
    using CookWithMe.Services.Mapping;

    public class AdministratorServiceModel : IMapTo<ApplicationUser>, IMapFrom<ApplicationUser>
    {
        public string Id { get; set; }

        public string Username { get; set; }

        public string FullName { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }
    }
}
