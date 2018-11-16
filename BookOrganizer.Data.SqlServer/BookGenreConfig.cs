using BookOrganizer.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookOrganizer.Data.SqlServer
{
    public class BookGenreConfig : IEntityTypeConfiguration<BookGenres>
    {
        public void Configure(EntityTypeBuilder<BookGenres> entity)
        {
            entity.HasOne(b => b.Book)
                .WithMany(gl => gl.GenreLink)
                .HasForeignKey(b => b.BookId);

            entity.HasOne(g => g.Genre)
                .WithMany(bl => bl.BookLink)
                .HasForeignKey(g => g.GenreId);
        }
    }
}
