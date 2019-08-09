namespace CookWithMe.Services
{
    using System;

    public interface IEnumParseService
    {
        string GetEnumDescription(string name, Type typeOfEnum);

        TEnum Parse<TEnum>(string description, Type typeOfEnum);
    }
}
