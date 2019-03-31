using BookOrganizer.Data.SqlServer;
using BookOrganizer.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace BookOrganizer.UI.WPF.Repositories
{
    public class SeriesRepository : BaseRepository<Series, BookOrganizerDbContext>
    {
        public SeriesRepository(BookOrganizerDbContext context) : base(context)
        {

        }

        public async override Task<Series> GetSelectedAsync(Guid id)
        {
            if (id != Guid.Parse("00000000-0000-0000-0000-000000000000"))
                return await context.Series
                    .Include(b => b.BooksInSeries)
                    .Include(b => b.SeriesReadOrder)
                    .FirstOrDefaultAsync(b => b.Id == id);

            else return new Series();
        }
    }
}
