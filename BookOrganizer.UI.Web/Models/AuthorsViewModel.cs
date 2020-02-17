using System.Collections.Generic;
using BookOrganizer.Domain;

namespace BookOrganizer.UI.Web.Models
{
    public class AuthorsViewModel
    {
        public IEnumerable<LookupItem> Authors { get; set; }
    }
}
