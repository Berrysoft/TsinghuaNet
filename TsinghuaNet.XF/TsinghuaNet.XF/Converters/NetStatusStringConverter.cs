using System;
using System.Globalization;
using TsinghuaNet.Models;
using Xamarin.Forms;

namespace TsinghuaNet.XF.Converters
{
    class NetStatusStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch ((NetStatus)value)
            {
                case NetStatus.Wwan:
                    return "移动流量";
                case NetStatus.Wlan:
                    return "无线网络";
                case NetStatus.Lan:
                    return "有线网络";
                default:
                    return "未知";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}
