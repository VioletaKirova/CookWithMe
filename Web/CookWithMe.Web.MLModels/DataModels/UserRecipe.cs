namespace CookWithMe.Web.MLModels.DataModels
{
    using Microsoft.ML.Data;

    public class UserRecipe
    {
        [LoadColumn(0)]
        public string UserId { get; set; }

        [LoadColumn(1)]
        public string RecipeId { get; set; }

        [LoadColumn(2)]
        public float Label { get; set; }
    }
}
