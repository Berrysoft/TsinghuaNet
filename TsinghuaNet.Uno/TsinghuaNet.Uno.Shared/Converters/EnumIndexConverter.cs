using System;
using Windows.UI.Xaml.Data;

namespace TsinghuaNet.Uno.Converters
{
    public class EnumIndexConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (Enum.IsDefined(value.GetType(), value))
                return value;
            else
                return 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
            => value;
    }
}
