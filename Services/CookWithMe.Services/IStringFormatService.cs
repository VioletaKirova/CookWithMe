namespace CookWithMe.Services
{
    using System.Collections.Generic;

    public interface IStringFormatService
    {
        string DisplayTime(int time);

        IList<string> SplitBySemicollonAndWhitespace(string text);

        string RemoveWhiteSpaces(string text);
    }
}
