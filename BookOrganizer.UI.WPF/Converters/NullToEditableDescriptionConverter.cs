using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace BookOrganizer.UI.WPF.Converters
{
    public class NullToEditableDescriptionConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            return (values[0] is null && (bool)values[1] == false)
                ? Visibility.Hidden
                : (object)Visibility.Visible;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
