using BookOrganizer.Data.SqlServer;
using BookOrganizer.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace BookOrganizer.DA
{
    public class GenreRepository : BaseRepository<Genre, BookOrganizerDbContext>, IGenreRepository
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

        public async Task AddNewGenreAsync(Genre genre)
        {
            context.Genres.Add(genre);
            await context.SaveChangesAsync();
        }
    }
}
