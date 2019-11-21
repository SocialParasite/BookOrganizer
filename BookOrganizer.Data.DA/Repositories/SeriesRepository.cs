using BookOrganizer.Data.SqlServer;
using BookOrganizer.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace BookOrganizer.DA
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
                    .Include(b => b.BooksSeries)
                    .ThenInclude(s => s.Series)
                    .ThenInclude(sr => sr.SeriesReadOrder)
                    .ThenInclude(b => b.Book)
                    .Include(b => b.SeriesReadOrder)
                    .ThenInclude(b => b.Book)
                    .FirstOrDefaultAsync(b => b.Id == id)
                : new Series();
        }
    }
}
