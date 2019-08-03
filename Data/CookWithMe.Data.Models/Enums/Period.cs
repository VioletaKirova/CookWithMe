namespace CookWithMe.Data.Models.Enums
{
    using System.ComponentModel;

    public enum Period
    {
        [Description("All")]
        All = 1,

        [Description("A La Minute")]
        ALaMinute = 2,

        [Description("Half An Hour")]
        HalfAnHour = 3,

        [Description("One Hour")]
        OneHour = 4,

        [Description("Over One Hour")]
        OverOneHour = 5,
    }
}
