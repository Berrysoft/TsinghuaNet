using System;
using TsinghuaNet.Helpers;
using Windows.UI.Xaml.Data;

namespace TsinghuaNet.Uno.Converters
{
    public class CurrencyStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
            => StringHelper.GetCurrencyString((decimal)value);

        public object ConvertBack(object value, Type targetType, object parameter, string language)
            => throw new NotImplementedException();
    }
}
