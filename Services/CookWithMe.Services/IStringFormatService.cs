namespace CookWithMe.Services
{
    using System.Collections.Generic;

    public interface IStringFormatService
    {
        string DisplayTime(int time);

        IList<string> SplitBySemicollonAndWhitespace(string text);

        IList<string> SplitByCommaAndWhitespace(string text);

        string RemoveWhiteSpaces(string text);
    }
}
