namespace CookWithMe.Services.Models.Lifestyles
{
    using CookWithMe.Data.Models;
    using CookWithMe.Services.Mapping;

    public class LifestyleServiceModel : IMapTo<Lifestyle>, IMapFrom<Lifestyle>
    {
        public int Id { get; set; }

        public string Type { get; set; }
    }
}
