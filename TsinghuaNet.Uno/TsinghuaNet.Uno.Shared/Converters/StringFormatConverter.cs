using System;
using Windows.UI.Xaml.Data;

namespace TsinghuaNet.Uno.Converters
{
    public class StringFormatConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
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

        public object ConvertBack(object value, Type targetType, object parameter, string language)
            => throw new NotImplementedException();
    }
}
