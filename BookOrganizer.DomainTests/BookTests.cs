using BookOrganizer.Domain;
using FluentAssertions;
using Xunit;

namespace BookOrganizer.DomainTests
{
    public class BookTests
    { 
        [Fact]
        public void BookDescriptionShouldAcceptStringInput()
        {
            var book = new Book();
            book.Description = string.Empty;

            book.Description.Should().BeOfType<string>();
            book.Description.Should().BeEmpty();
        }

        [Fact]
        public void BookIsReadShouldAcceptBoolInput()
        {
            var book = new Book();
            book.IsRead = true;

            book.IsRead.Should().BeTrue();
        }

        [Fact]
        public void BookCoverPicturesAreStoredUnderCurrentUsersProfileInCoversSubfolder()
        {
            var book = new Book();

            book.BookCoverPicturePath = @"C:\temp\testingBookCoverPath\fake.jpg";

            var test = "C:\\Users\\tonij\\Pictures\\BookOrganizer\\Covers\\fake.jpg";

            book.BookCoverPicturePath.Should().Equals(test);
        }
    }
}
