using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookOrganizer.DA;
using BookOrganizer.Domain;

namespace BookOrganizer.UI.Web.Models
{
    public class PublisherDetailViewModel : BaseDetailViewModel<Publisher>
    {
        public PublisherDetailViewModel(Publisher selectedPublisher)
        {
            SelectedItem = selectedPublisher ?? throw new ArgumentNullException(nameof(selectedPublisher));
        }

        public List<LookupItem> Books { get; set; }
    }
}
