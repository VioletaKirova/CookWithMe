namespace CookWithMe.Services
{
    using System.Collections.Generic;

    public interface IStringFormatService
    {
        IList<string> SplitBySemicollonAndWhitespace(string text);

        string RemoveWhiteSpaces(string text);
    }
}
