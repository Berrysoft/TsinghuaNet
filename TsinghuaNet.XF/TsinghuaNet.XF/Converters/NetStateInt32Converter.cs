using System;
using System.Globalization;
using Xamarin.Forms;

namespace TsinghuaNet.XF.Converters
{
    class NetStateInt32Converter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => value;

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => (NetState)(int)value;
    }
}
