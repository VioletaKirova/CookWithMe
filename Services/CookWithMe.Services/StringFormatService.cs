namespace CookWithMe.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class StringFormatService : IStringFormatService
    {
        public string FormatTime(int time)
        {
            int hours = time / 60;
            int minutes = time % 60;

            return hours == 0 ?
                $" {minutes} min" : minutes == 0 ?
                $" {hours} h" :
                $" {hours} h {minutes} min";
        }

        public string RemoveWhitespaces(string text)
        {
            return text.Replace(" ", string.Empty);
        }

        public IList<string> SplitByCommaAndWhitespace(string text)
        {
            return text.Split(
                new string[] { ",", " ", ", " },
                StringSplitOptions.RemoveEmptyEntries);
        }

        public IList<string> SplitBySemicollon(string text)
        {
            return text.Split(
                new string[] { ";" },
                StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.Trim())
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .ToList();
        }
    }
}
