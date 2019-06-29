using System;
using System.Globalization;
using System.Windows.Data;

namespace BookOrganizer.UI.WPF.Converters
{
    [ValueConversion(typeof(DateTime), typeof(string))]
    public class DobCountAge : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return $"{value:dd.MM.yyyy} ({(int)Math.Floor((DateTime.Now - (DateTime)value).TotalDays / 365.25D)} years)";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
