namespace CookWithMe.Common
{
    public class AttributesConstraints
    {
        // Users
        public const int UsernameMaxLength = 256;

        public const int FullNameMinLength = 3;
        public const int FullNameMaxLength = 50;

        public const int EmailMinLength = 3;
        public const int EmailMaxLength = 256;

        public const int PasswordMinLength = 6;
        public const int PasswordMaxLength = 100;

        public const int BiographyMaxLength = 200;

        public const int UserLifestyleTypeMaxLength = 20;

        // Recipe
        public const int RecipeTitleMinLength = 3;
        public const int RecipeTitleMaxLength = 50;

        public const int RecipeSummaryMinLength = 10;
        public const int RecipeSummaryMaxLength = 1000;

        public const int RecipeTimeMinValue = 1;
        public const int RecipeTimeMaxValue = 300;

        public const double RecipeYieldMinValue = 0.1;
        public const double RecipeYieldMaxValue = 10000;

        // Browse
        public const int BrowseSingleWordFieldsMaxLength = 20;
        public const int BrowseMultipleWordFieldsMaxLength = 200;

        public const double BrowseYieldMinValue = 0.1;
        public const double BrowseYieldMaxValue = 10000;

        public const double BrowseNutritionalValueMinValue = 0.1;
        public const double BrowseNutritionalValueMaxValue = 10000;

        // Category
        public const int CategoryTitleMinLength = 3;
        public const int CategoryTitleMaxLength = 20;

        // NutritionalValue
        public const double NutritionalValueMinValue = 0.1;
        public const double NutritionalValueMaxValue = 10000;

        // Reviews
        public const int ReviewCommentMinLength = 1;
        public const int ReviewCommentMaxLength = 250;

        public const int ReviewRatingMinValue = 1;
        public const int ReviewRatingMaxValue = 5;

    }
}
