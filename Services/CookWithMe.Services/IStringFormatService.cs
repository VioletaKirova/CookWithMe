﻿namespace CookWithMe.Services
{
    using System.Collections.Generic;

    public interface IStringFormatService
    {
        string FormatTime(int time);

        IList<string> SplitBySemicollon(string text);

        IList<string> SplitByCommaAndWhitespace(string text);

        string RemoveWhitespaces(string text);
    }
}
