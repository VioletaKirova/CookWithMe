namespace CookWithMe.Data.Models.Enums
{
    using System.ComponentModel;

    public enum Period
    {
        [Description("A La Minute")]
        ALaMinute = 1,

        [Description("Half An Hour")]
        HalfAnHour = 2,

        [Description("One Hour")]
        OneHour = 3,

        [Description("Over One Hour")]
        OverOneHour = 4,
    }
}
