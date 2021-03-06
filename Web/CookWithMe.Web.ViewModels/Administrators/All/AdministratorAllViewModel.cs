﻿namespace CookWithMe.Web.ViewModels.Administrators.All
{
    using CookWithMe.Services.Mapping;
    using CookWithMe.Services.Models.Administrators;

    public class AdministratorAllViewModel : IMapFrom<AdministratorServiceModel>
    {
        public string Id { get; set; }

        public string Username { get; set; }

        public string FullName { get; set; }
    }
}
