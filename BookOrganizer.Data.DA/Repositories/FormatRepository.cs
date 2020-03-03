using BookOrganizer.Data.SqlServer;
using BookOrganizer.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace BookOrganizer.DA
{
    public class FormatRepository : BaseRepository<Format, BookOrganizerDbContext>, IFormatRepository
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

        public async Task AddNewFormatAsync(Format format)
        {
            context.Formats.Add(format);
            await context.SaveChangesAsync();
        }
    }
}
