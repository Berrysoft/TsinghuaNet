using System;
using System.Globalization;
using Xamarin.Forms;
using TsinghuaNet.Helper;

namespace TsinghuaNet.CrossPlatform.Converters
{
    class FluxStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            => StringHelper.GetFluxString((long)value);

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}
