namespace CookWithMe.Services
{
    using System;
    using System.Collections.Generic;

    public class StringFormatService : IStringFormatService
    {
        public string RemoveWhiteSpaces(string text)
        {
            return text.Replace(" ", string.Empty);
        }

        public IList<string> SplitBySemicollonAndWhitespace(string text)
        {
            return text.Split(new string[] { ";", "; " }, StringSplitOptions.RemoveEmptyEntries);
        }

    }
}
