using BookOrganizer.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookOrganizer.Data.SqlServer
{
    public class BookFormatConfig : IEntityTypeConfiguration<BooksFormats>
    {
        public void Configure(EntityTypeBuilder<BooksFormats> entity)
        {
            entity.HasOne(b => b.Book)
                .WithMany(fl => fl.FormatLink)
                .HasForeignKey(b => b.BookId);

            entity.HasOne(f => f.Format)
                .WithMany(bl => bl.BookLink)
                .HasForeignKey(f => f.FormatId);
        }
    }
}
