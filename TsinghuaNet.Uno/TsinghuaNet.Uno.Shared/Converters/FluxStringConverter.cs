using System;
using TsinghuaNet.Models;
using Windows.UI.Xaml.Data;

namespace TsinghuaNet.Uno.Converters
{
    public class FluxStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
            => $"{(long)((ByteSize)value).GigaBytes} GB";

        public object ConvertBack(object value, Type targetType, object parameter, string language)
            => throw new NotImplementedException();
    }

    public class FluxLimitConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
            => ((ByteSize)value).GigaBytes;

        public object ConvertBack(object value, Type targetType, object parameter, string language)
            => ByteSize.FromGigaBytes((double)value);
    }
}
