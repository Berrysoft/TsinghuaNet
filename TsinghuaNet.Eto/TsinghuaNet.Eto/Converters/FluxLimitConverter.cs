using System;
using System.Globalization;
using Eto.Forms;

namespace TsinghuaNet.Eto.Converters
{
    public class FluxLimitConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            ByteSize bytes = (ByteSize)value;
            return bytes.GigaBytes;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ByteSize.FromGigaBytes((double)value);
        }
    }
}
