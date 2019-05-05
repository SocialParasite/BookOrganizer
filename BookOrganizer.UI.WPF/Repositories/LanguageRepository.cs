using BookOrganizer.Data.SqlServer;
using BookOrganizer.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace BookOrganizer.UI.WPF.Repositories
{
    public class LanguageRepository : BaseRepository<Language, BookOrganizerDbContext>
    {
        public LanguageRepository(BookOrganizerDbContext context) : base(context)
        {
        }

        public async override Task<Language> GetSelectedAsync(Guid id)
        {
            return id != Guid.Parse("00000000-0000-0000-0000-000000000000")
                ? await context.Languages
                    .FirstOrDefaultAsync(l => l.Id == id)
                : new Language();
        }
    }
}
