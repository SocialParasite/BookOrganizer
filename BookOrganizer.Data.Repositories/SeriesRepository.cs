using BookOrganizer.Data.SqlServer;
using BookOrganizer.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace BookOrganizer.Data.Repositories
{
    public class SeriesRepository : BaseRepository<Series, BookOrganizerDbContext>, ISeriesRepository
    {
        public SeriesRepository(BookOrganizerDbContext context) : base(context)
        {

        }

        public async Task<Book> GetBookById(Guid bookId)
        {
            return await context.Books.SingleAsync(b => b.Id == bookId);
        }

        public async override Task<Series> GetSelectedAsync(Guid id)
        {
            return id != default
                ? await context.Series
                    .Include(b => b.BooksInSeries)
                    .Include(b => b.SeriesReadOrder)
                    .FirstOrDefaultAsync(b => b.Id == id)
                : new Series();
        }
    }
}
