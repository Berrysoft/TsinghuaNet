using System;
using System.Globalization;
using Xamarin.Forms;
using TsinghuaNet.Helper;

namespace TsinghuaNet.CrossPlatform.Converters
{
    class BalanceStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            => StringHelper.GetCurrencyString((decimal)value);

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}
