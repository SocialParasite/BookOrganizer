using System.Collections.Generic;
using BookOrganizer.Domain;

namespace BookOrganizer.UI.Web.Models
{
    public class SeriesViewModel
    {
        public IEnumerable<LookupItem> Series { get; set; }
    }
}
