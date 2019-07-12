using System;
using Windows.ApplicationModel;
using Windows.UI.Xaml.Data;

namespace TsinghuaNet.Uno.Converters
{
    public class PackageVersionStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
#if WINDOWS_UWP
            PackageVersion ver = (PackageVersion)value;
            return $"版本 {ver.Major}.{ver.Minor}.{ver.Build}.{ver.Revision}";
#else
            return value.ToString();
#endif
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
