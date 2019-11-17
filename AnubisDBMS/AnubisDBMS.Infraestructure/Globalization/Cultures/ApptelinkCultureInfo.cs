using System.Globalization;

namespace AnubisDBMS.Infraestructure.Globalization.Cultures
{
    public static class GenericCultureInfo
    {
        public static CultureInfo GetCultureInfo(GenericCultureOptions options = null)
        {
            options = options ?? new GenericCultureOptions();
            var culture = (CultureInfo) CultureInfo.GetCultureInfo(options.DefaultCulture).Clone();
            culture.DateTimeFormat.ShortDatePattern = options.ShortDateFormat;
            culture.DateTimeFormat.LongDatePattern = options.LongDateFormat;
            return culture;
        }
    }

    public class GenericCultureOptions
    {
        public GenericCultureOptions()
        {
            DefaultCulture = "es-EC";
            ShortDateFormat = "dd-MM-yyyy";
            LongDateFormat = "MMMM dd, yyyy";
        }

        public string DefaultCulture { get; set; }
        public string ShortDateFormat { get; set; }
        public string LongDateFormat { get; set; }
    }
}