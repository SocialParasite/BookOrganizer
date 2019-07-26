using BookOrganizer.Data.SqlServer;
using BookOrganizer.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace BookOrganizer.Data.Repositories
{
    public class GenreRepository : BaseRepository<Genre, BookOrganizerDbContext>
    {
        public GenreRepository(BookOrganizerDbContext context) : base(context)
        {
        }

        public async override Task<Genre> GetSelectedAsync(Guid id)
        {
            return id != default
                ? await context.Genres
                    .FirstOrDefaultAsync(f => f.Id == id)
                : new Genre();
        }
    }
}
