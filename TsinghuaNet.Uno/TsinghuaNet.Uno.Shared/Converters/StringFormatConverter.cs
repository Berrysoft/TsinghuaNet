using System;
using System.Globalization;
using Windows.UI.Xaml.Data;

namespace TsinghuaNet.Uno.Converters
{
    public class StringFormatConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var format = parameter?.ToString();
            var culture = CultureInfo.GetCultureInfo(language);
            if (string.IsNullOrEmpty(format))
                return string.Format(culture, "{0}", value);
            else
                return string.Format(culture, format, value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
            => throw new NotImplementedException();
    }
}
