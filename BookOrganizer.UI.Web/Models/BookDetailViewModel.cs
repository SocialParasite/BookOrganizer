using System;
using BookOrganizer.Domain;

namespace BookOrganizer.UI.Web.Models
{
    public class BookDetailViewModel : BaseDetailViewModel<Book>
    {
        public BookDetailViewModel(Book book)
        {
            SelectedItem = book ?? throw new ArgumentNullException(nameof(book));
        }
    }
}
