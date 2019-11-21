using BookOrganizer.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookOrganizer.Data.SqlServer
{
    public class BookSeriesConfig : IEntityTypeConfiguration<BooksSeries>
    {
        public void Configure(EntityTypeBuilder<BooksSeries> entity)
        {
            entity.HasOne(b => b.Book)
                .WithMany(bs => bs.BooksSeries)
                .HasForeignKey(b => b.BookId);

            entity.HasOne(s => s.Series)
                .WithMany(bs => bs.BooksSeries)
                .HasForeignKey(s => s.SeriesId);
        }
    }
}
