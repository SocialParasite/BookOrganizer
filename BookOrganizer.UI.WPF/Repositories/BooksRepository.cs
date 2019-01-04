using BookOrganizer.Data.SqlServer;
using BookOrganizer.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BookOrganizer.UI.WPF.Repositories
{
    public class BooksRepository : BaseRepository<Book, BookOrganizerDbContext>
    {
        private readonly BookOrganizerDbContext context;

        public BooksRepository(BookOrganizerDbContext context) : base(context)
        {
            this.context = context;
        }

        public Book GetBookByTitle(string title)
            => context.Books.Include(b => b.Publisher).FirstOrDefault(b => b.Title == title);

        public async override Task<Book> GetSelectedAsync(Guid id)
        {
            return await context.Books
                .Include(b => b.Publisher)
                .Include(b => b.Language)
                .Include(b => b.AuthorsLink)
                .ThenInclude(a => a.Author)
                .Include(b => b.BookSeries)
                .ThenInclude(s => s.BooksInSeries)
                .Include(b => b.BookSeries)
                .ThenInclude(s => s.SeriesReadOrder)
                .FirstOrDefaultAsync(b => b.Id == id);
        }
    }
}
