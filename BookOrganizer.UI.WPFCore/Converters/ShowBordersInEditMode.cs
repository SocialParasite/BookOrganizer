using System;
using System.Globalization;
using System.Windows.Data;

namespace BookOrganizer.UI.WPFCore.Converters
{
    public class ShowBordersInEditMode : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) 
            => ((bool)value) ? 1 : 0;

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => null;
    }
}
