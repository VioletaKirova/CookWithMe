namespace CookWithMe.Services.Data
{
    using System.Threading.Tasks;

    public interface INutritionalValueService
    {
        Task<string> GetIdByRecipeId(string recipeId);
    }
}
