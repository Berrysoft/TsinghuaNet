using System;
using System.Globalization;
using Xamarin.Forms;

namespace TsinghuaNet.XF.Converters
{
    class EnumInt32Converter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => value;

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => Enum.GetValues(targetType).GetValue((int)value);
    }
}
