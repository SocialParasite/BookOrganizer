using BookOrganizer.Data.Lookups;
using System.Collections.Generic;

namespace BookOrganizer.UI.Web.Models
{
    public class AuthorsViewModel
    {
        public IEnumerable<LookupItem> Authors { get; set; }
    }
}
