using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace BookOrganizer.UI.WPFCore.Converters
{
    public class BackgroundColorConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if ((bool)values[0] && (bool)values[1])
                return new SolidColorBrush(Colors.GreenYellow);
            else if ((bool)values[0] && !(bool)values[1])
                return new SolidColorBrush(Colors.Aquamarine);
            else if (!(bool)values[0] && (bool)values[1])
                return new SolidColorBrush(Colors.Crimson);

            return new SolidColorBrush(Colors.WhiteSmoke);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
