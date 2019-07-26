using BookOrganizer.Data.SqlServer;
using BookOrganizer.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookOrganizer.Data.Repositories
{
    public class NationalityRepository : BaseRepository<Nationality, BookOrganizerDbContext>
    {
        public NationalityRepository(BookOrganizerDbContext context) : base(context)
        {
        }

        public async override Task<Nationality> GetSelectedAsync(Guid id)
        {
            return id != default
                ? await context.Nationalities
                    .FirstOrDefaultAsync(n=> n.Id == id)
                : new Nationality();
        }
    }
}
