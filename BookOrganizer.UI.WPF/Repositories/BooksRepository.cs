using BookOrganizer.Data.SqlServer;
using BookOrganizer.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BookOrganizer.UI.WPF.Repositories
{
    public class BooksRepository : BaseRepository<Book, BookOrganizerDbContext>, IBookRepository
    {
        public BooksRepository(BookOrganizerDbContext context) : base(context)
        {

        }

        public Book GetBookByTitle(string title)
            => context.Books
                .Include(b => b.Publisher)
                .FirstOrDefault(b => b.Title == title);

        public async override Task<Book> GetSelectedAsync(Guid id)
        {
            return id != Guid.Parse("00000000-0000-0000-0000-000000000000")
                ? await context.Books
                    .Include(b => b.Publisher)
                    .Include(b => b.Language)
                    .Include(b => b.AuthorsLink)
                    .ThenInclude(a => a.Author)
                    .Include(b => b.BookSeries)
                    .ThenInclude(s => s.BooksInSeries)
                    .Include(b => b.BookSeries)
                    .ThenInclude(s => s.SeriesReadOrder)
                    .Include(b => b.ReadDates)
                    .FirstOrDefaultAsync(b => b.Id == id)
                : new Book();
        }

        public async Task<Author> GetBookAuthorById(Guid authorId)
            => await context.Authors.SingleAsync(a => a.Id == authorId);
    }
}
