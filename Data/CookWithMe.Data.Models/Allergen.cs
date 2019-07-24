namespace CookWithMe.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using CookWithMe.Data.Common.Models;

    public class Allergen : BaseModel<int>
    {
        public Allergen()
        {
            this.CreatedOn = DateTime.UtcNow;
        }

        [Required]
        [MaxLength(20)]
        public string Name { get; set; }
    }
}
