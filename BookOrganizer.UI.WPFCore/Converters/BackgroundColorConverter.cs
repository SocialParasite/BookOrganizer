using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using BookOrganizer.Domain.Enums;

namespace BookOrganizer.UI.WPFCore.Converters
{
    public class BackgroundColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch (value)
            {
                case BookStatus _:
                    return value switch
                    {
                        BookStatus.Read | BookStatus.Owned => new SolidColorBrush(Colors.PaleGreen),
                        BookStatus.Read => new SolidColorBrush(Colors.PaleVioletRed),
                        BookStatus.Owned => new SolidColorBrush(Colors.LightSkyBlue),
                        _ => new SolidColorBrush(Colors.WhiteSmoke),
                    };
                case SeriesStatus _:
                    return value switch
                    {
                        SeriesStatus.NoneOwnedNoneRead => new SolidColorBrush(Colors.Red),
                        SeriesStatus.NoneOwnedPartlyRead => new SolidColorBrush(Colors.OrangeRed),
                        SeriesStatus.NoneOwnedAllRead => new SolidColorBrush(Colors.PaleVioletRed),
                        SeriesStatus.PartlyOwnedNoneRead => new SolidColorBrush(Colors.AliceBlue),
                        SeriesStatus.PartlyOwnedPartlyRead => new SolidColorBrush(Colors.LightSteelBlue),
                        SeriesStatus.PartlyOwnedAllRead => new SolidColorBrush(Colors.CornflowerBlue),
                        SeriesStatus.AllOwnedNoneRead => new SolidColorBrush(Colors.LightSkyBlue),
                        SeriesStatus.AllOwnedPartlyRead => new SolidColorBrush(Colors.DeepSkyBlue),
                        SeriesStatus.AllOwnedAllRead => new SolidColorBrush(Colors.PaleGreen),
                        _ => new SolidColorBrush(Colors.WhiteSmoke),
                    };
                default:
                    return new SolidColorBrush(Colors.WhiteSmoke);
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
