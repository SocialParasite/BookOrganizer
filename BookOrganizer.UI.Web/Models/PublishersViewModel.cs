using System.Collections.Generic;
using BookOrganizer.DA;

namespace BookOrganizer.UI.Web.Models
{
    public class PublishersViewModel
    {
        public IEnumerable<LookupItem> Publishers { get; set; }
    }
}
