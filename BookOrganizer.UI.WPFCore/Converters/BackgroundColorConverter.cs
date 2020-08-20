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
                    switch (value)
                    {
                        case BookStatus.Read | BookStatus.Owned:
                            return new SolidColorBrush(Colors.GreenYellow);
                        case BookStatus.Read:
                            return new SolidColorBrush(Colors.Aquamarine);
                        case BookStatus.Owned:
                            return new SolidColorBrush(Colors.Crimson);
                        default:
                            return new SolidColorBrush(Colors.WhiteSmoke);

                    }
                case SeriesStatus _:
                    switch (value)
                    {
                        case SeriesStatus.NoneOwnedNoneRead:
                        case SeriesStatus.NoneOwnedPartlyRead:
                        case SeriesStatus.NoneOwnedAllRead:
                            return new SolidColorBrush(Colors.GreenYellow);
                        case SeriesStatus.PartlyOwnedNoneRead:
                        case SeriesStatus.PartlyOwnedPartlyRead:
                        case SeriesStatus.PartlyOwnedAllRead:
                            return new SolidColorBrush(Colors.Aquamarine);
                        case SeriesStatus.AllOwnedNoneRead:
                        case SeriesStatus.AllOwnedPartlyRead:
                        case SeriesStatus.AllOwnedAllRead:
                            return new SolidColorBrush(Colors.Crimson);
                        default:
                            return new SolidColorBrush(Colors.WhiteSmoke);

                    }
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
