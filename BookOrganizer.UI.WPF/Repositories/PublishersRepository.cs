using BookOrganizer.Data.SqlServer;
using BookOrganizer.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace BookOrganizer.UI.WPF.Repositories
{
    public class PublishersRepository : BaseRepository<Publisher, BookOrganizerDbContext>
    {
        public PublishersRepository(BookOrganizerDbContext context) : base(context)
        {
        }

        public async override Task<Publisher> GetSelectedAsync(Guid id)
        {
            if (id != Guid.Parse("00000000-0000-0000-0000-000000000000"))
                return await context.Publishers
                    .Include(b => b.Books)
                    .FirstOrDefaultAsync(b => b.Id == id);

            else return new Publisher();
        }
    }
}
