using System;
using BookOrganizer.Domain;

namespace BookOrganizer.UI.Web.Models
{
    public class SeriesDetailViewModel : BaseDetailViewModel<Series>
    {
        public SeriesDetailViewModel(Series series)
        {
            SelectedItem = series ?? throw new ArgumentNullException(nameof(series));
        }
    }
}
