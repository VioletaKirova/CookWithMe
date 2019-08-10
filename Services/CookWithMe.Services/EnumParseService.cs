namespace CookWithMe.Services
{
    using System;
    using System.ComponentModel;
    using System.Reflection;

    public class EnumParseService : IEnumParseService
    {
        private readonly IStringFormatService stringFormatService;

        public EnumParseService(IStringFormatService stringFormatService)
        {
            this.stringFormatService = stringFormatService;
        }

        public string GetEnumDescription(string name, Type typeOfEnum)
        {
            FieldInfo specificField = typeOfEnum.GetField(name);

            if (specificField != null)
            {
                DescriptionAttribute attr =
                       Attribute.GetCustomAttribute(
                           specificField,
                           typeof(DescriptionAttribute)) as DescriptionAttribute;

                if (attr != null)
                {
                    return attr.Description;
                }
            }

            return null;
        }

        public TEnum Parse<TEnum>(string description)
        {
            return (TEnum)Enum.Parse(
                            typeof(TEnum),
                            this.stringFormatService.RemoveWhitespaces(description));
        }
    }
}
