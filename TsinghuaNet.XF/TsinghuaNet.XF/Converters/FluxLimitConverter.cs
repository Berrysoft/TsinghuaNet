using System;
using System.Globalization;
using TsinghuaNet.Models;
using Xamarin.Forms;

namespace TsinghuaNet.XF.Converters
{
    class FluxLimitConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            => ((ByteSize)value).GigaBytes;

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => ByteSize.FromGigaBytes((double)value);
    }
}
