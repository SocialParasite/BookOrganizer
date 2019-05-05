using System;
using System.Globalization;
using System.Windows.Data;

namespace BookOrganizer.UI.WPF.Converters
{
    [ValueConversion(typeof(bool), typeof(double))]
    public class InvertedBooleanToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool)
            {
                return (!(bool)value);
            }
            else
            {
                throw new ArgumentException("Value must be of the type bool");
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool)
            {
                return (!(bool)value);
            }
            else
            {
                throw new ArgumentException();
            }
        }
    }
}
