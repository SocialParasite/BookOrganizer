using BookOrganizer.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookOrganizer.UI.Web.Models
{
    public class BookDetailsViewModel : BaseDetailViewModel<Book>
    {
        public BookDetailsViewModel(Book book)
        {
            SelectedItem = book ?? throw new ArgumentNullException(nameof(book));
        }
    }
}
