using BookOrganizer.Data.SqlServer;
using BookOrganizer.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BookOrganizer.DA
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

        public override async Task<Book> GetSelectedAsync(Guid id)
        {
            return id != default
                ? await context.Books
                    .Include(b => b.Publisher)
                    .Include(b => b.Language)
                    .Include(b => b.AuthorsLink)
                    .ThenInclude(a => a.Author)
                    .Include(b => b.BooksSeries)
                    .ThenInclude(s => s.Series)
                    .ThenInclude(sr => sr.SeriesReadOrder)
                    .ThenInclude(b => b.Book)
                    .Include(b => b.ReadDates)
                    .Include(b => b.FormatLink)
                    .ThenInclude(f => f.Format)
                    .Include(g => g.GenreLink)
                    .ThenInclude(g => g.Genre)
                    .FirstOrDefaultAsync(b => b.Id == id)
                : new Book();
        }

        public async Task<Author> GetBookAuthorById(Guid authorId)
            => await context.Authors.SingleAsync(a => a.Id == authorId);

        public async Task<Format> GetBookFormatById(Guid formatId)
            => await context.Formats.SingleAsync(f => f.Id == formatId);

        public async Task<Genre> GetBookGenreById(Guid genreId)
            => await context.Genres.SingleAsync(g => g.Id == genreId);
    }
}
