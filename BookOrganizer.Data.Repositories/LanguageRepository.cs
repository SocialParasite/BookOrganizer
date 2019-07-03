using BookOrganizer.Data.SqlServer;
using BookOrganizer.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookOrganizer.Data.Repositories
{
    public class LanguageRepository : BaseRepository<Language, BookOrganizerDbContext>
    {
        public LanguageRepository(BookOrganizerDbContext context) : base(context)
        {
        }

        public async override Task<Language> GetSelectedAsync(Guid id)
        {
            return id != default
                ? await context.Languages
                    .FirstOrDefaultAsync(l => l.Id == id)
                : new Language();
        }
    }
}
