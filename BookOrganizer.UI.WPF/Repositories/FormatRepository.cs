using BookOrganizer.Data.SqlServer;
using BookOrganizer.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace BookOrganizer.UI.WPF.Repositories
{
    public class FormatRepository : BaseRepository<Format, BookOrganizerDbContext>
    {
        public FormatRepository(BookOrganizerDbContext context) : base(context)
        {
        }

        public async override Task<Format> GetSelectedAsync(Guid id)
        {
            return id != default
                ? await context.Formats
                    .FirstOrDefaultAsync(f => f.Id == id)
                : new Format();
        }
    }
}
