namespace CookWithMe.Common
{
    public class AttributesErrorMessages
    {
        public const string RequiredErrorMessage = "Please fill in \"{0}\".";

        public const string CompareErrorMessage = "\"{1}\" and \"{0}\" do not match.";

        public const string StringLengthErrorMessage = "\"{0}\" must be at least {2} and at max {1} characters long.";

        public const string MaxLengthErrorMessage = "\"{0}\" can be maximum {1} characters long.";

        public const string RangeErrorMessage = "\"{0}\" must be between {1} and {2}.";

        public const string EnsureMinimumElementsErrorMessage = "\"{0}\" must be at least 1.";
    }
}
