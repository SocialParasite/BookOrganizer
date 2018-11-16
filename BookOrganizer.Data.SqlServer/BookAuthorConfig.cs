using BookOrganizer.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookOrganizer.Data.SqlServer
{
    public class BookAuthorConfig : IEntityTypeConfiguration<BookAuthors>
    {
        public void Configure(EntityTypeBuilder<BookAuthors> entity)
        {
            entity.HasOne(b => b.Book)
                .WithMany(al => al.AuthorsLink)
                .HasForeignKey(b => b.BookId);

            entity.HasOne(a => a.Author)
                .WithMany(bl => bl.BooksLink)
                .HasForeignKey(a => a.AuthorId);
        }
    }
}
