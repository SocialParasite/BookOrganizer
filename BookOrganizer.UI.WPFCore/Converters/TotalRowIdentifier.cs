using System;
using System.Globalization;
using System.Windows.Data;

namespace BookOrganizer.UI.WPFCore.Converters
{
    public class TotalRowIdentifier : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((int)value == 0)
            {
                return "Total";
            }
            else
            {
                return value;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
