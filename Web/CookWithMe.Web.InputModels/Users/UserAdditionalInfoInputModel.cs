namespace CookWithMe.Web.InputModels.Users
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Microsoft.AspNetCore.Http;

    public class UserAdditionalInfoInputModel
    {
        [MaxLength(200)]
        public string Biography { get; set; }

        public IFormFile ProfilePhoto { get; set; }

        public string Lifestyle { get; set; }

        public IEnumerable<string> Allergies { get; set; }
    }
}
