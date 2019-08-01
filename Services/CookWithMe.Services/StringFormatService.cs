namespace CookWithMe.Services
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class StringFormatService : IStringFormatService
    {
        public ICollection<string> SplitBySemicollonAndWhitespace(string text)
        {
            return text.Split(new string[] { ";", "; " }, StringSplitOptions.RemoveEmptyEntries);
        }

        public string SplitByUppercaseLetter(string text)
        {
            var result = new StringBuilder();

            for (int i = 0; i < text.Length - 1; i++)
            {
                if ((char.IsLower(text[i]) && char.IsUpper(text[i + 1])) ||
                    (char.IsUpper(text[i]) && char.IsUpper(text[i + 1])))
                {
                    result.Append($"{text[i]} ");
                }
                else
                {
                    result.Append(text[i]);
                }
            }

            result.Append(text[text.Length - 1]);

            return result.ToString().TrimEnd();
        }
    }
}
