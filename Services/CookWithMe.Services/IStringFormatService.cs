namespace CookWithMe.Services
{
    using System.Collections.Generic;

    public interface IStringFormatService
    {
        string SplitByUppercaseLetter(string text);

        ICollection<string> SplitBySemicollonAndWhitespace(string text);
    }
}
