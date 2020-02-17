using System.Collections.Generic;
using BookOrganizer.Domain;

namespace BookOrganizer.UI.Web.Models
{
    public class PublishersViewModel
    {
        public IEnumerable<LookupItem> Publishers { get; set; }
    }
}
