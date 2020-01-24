using System.Collections.Generic;
using BookOrganizer.DA;

namespace BookOrganizer.UI.Web.Models
{
    public class SeriesViewModel
    {
        public IEnumerable<LookupItem> Series { get; set; }
    }
}
