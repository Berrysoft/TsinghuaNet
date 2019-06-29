using System;
using System.Globalization;
using Eto.Forms;

namespace TsinghuaNet.Eto.Converters
{
    public class StringFormatConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter == null)
                return value.ToString();
            else
            {
                var format = parameter.ToString();
                if (string.IsNullOrEmpty(format))
                    return value.ToString();
                else
                    return string.Format(format, value);
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
