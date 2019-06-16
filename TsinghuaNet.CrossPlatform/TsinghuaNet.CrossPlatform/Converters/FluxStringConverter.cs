using System;
using System.Globalization;
using Xamarin.Forms;

namespace TsinghuaNet.CrossPlatform.Converters
{
    class FluxStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            => ((ByteSize)value).ToString("F2");

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}
