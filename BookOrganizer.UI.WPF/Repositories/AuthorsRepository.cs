﻿using BookOrganizer.Data.SqlServer;
using BookOrganizer.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace BookOrganizer.UI.WPF.Repositories
{
    public class AuthorsRepository : BaseRepository<Author, BookOrganizerDbContext>
    {
        public AuthorsRepository(BookOrganizerDbContext context) : base(context)
        {
        }

        public async override Task<Author> GetSelectedAsync(Guid id)
        {
            if (id != Guid.Parse("00000000-0000-0000-0000-000000000000"))
                return await context.Authors
                    .Include(b => b.Nationality)
                    .Include(b => b.BooksLink)
                    .ThenInclude(bl => bl.Book)
                    .FirstOrDefaultAsync(b => b.Id == id);

            else return new Author();
        }
    }
}