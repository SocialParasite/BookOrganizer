using System.Collections.Generic;
using BookOrganizer.Domain;

namespace BookOrganizer.UI.Web.Models
{
    public class BooksViewModel
    {
        public IEnumerable<LookupItem> Books { get; set; }
    }
}
