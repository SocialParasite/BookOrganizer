using BookOrganizer.Data.Lookups;
using System.Collections.Generic;

namespace BookOrganizer.UI.Web.Models
{
    public class BooksViewModel
    {
        public IEnumerable<LookupItem> Books { get; set; }
    }
}
