namespace CookWithMe.Services
{
    using System;
    using System.Collections.Generic;

    public class StringFormatService : IStringFormatService
    {
        public string DisplayTime(int time)
        {
            int hours = time / 60;
            int minutes = time % 60;

            return hours == 0 ?
                $" {minutes} min" : minutes == 0 ?
                $" {hours} h" :
                $" {hours} h {minutes} min";
        }

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
