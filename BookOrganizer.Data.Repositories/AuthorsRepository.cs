using BookOrganizer.Data.SqlServer;
using BookOrganizer.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace BookOrganizer.Data.Repositories
{
    public class AuthorsRepository : BaseRepository<Author, BookOrganizerDbContext>
    {
        public AuthorsRepository(BookOrganizerDbContext context) : base(context)
        {
        }

        public async override Task<Author> GetSelectedAsync(Guid id)
        {
            if (id != default)
                return await context.Authors
                    .Include(b => b.Nationality)
                    .Include(b => b.BooksLink)
                    .ThenInclude(bl => bl.Book)
                    .FirstOrDefaultAsync(b => b.Id == id);

            else return new Author();
        }
    }
}
