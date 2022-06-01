using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace OrganizerWpf.Utilities.Converters
{
    public class DateTimeToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value != null ? ((DateTime)value).ToString("dd.MM.yyyy HH:mm") : string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            DateTime.TryParse(value.ToString(), out DateTime result);
            return result;
        }
    }
}
