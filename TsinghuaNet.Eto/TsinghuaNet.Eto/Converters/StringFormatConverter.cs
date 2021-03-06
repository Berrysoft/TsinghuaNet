﻿using System;
using System.Globalization;
using System.Threading;
using Eto.Forms;

namespace TsinghuaNet.Eto.Converters
{
    public class StringFormatConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var format = parameter?.ToString();
            if (culture == CultureInfo.InvariantCulture)
                culture = Thread.CurrentThread.CurrentCulture;
            if (string.IsNullOrEmpty(format))
                return string.Format(culture, "{0}", value);
            else
                return string.Format(culture, format, value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
