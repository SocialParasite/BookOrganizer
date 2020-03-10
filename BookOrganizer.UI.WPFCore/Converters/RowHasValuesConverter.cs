using System;
using System.Windows;
using System.Windows.Data;

namespace BookOrganizer.UI.WPFCore.Converters
{
    public class RowHasValuesConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value.GetType().Name == "NamedObject" 
                ? Visibility.Hidden 
                : (object)Visibility.Visible;

        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
