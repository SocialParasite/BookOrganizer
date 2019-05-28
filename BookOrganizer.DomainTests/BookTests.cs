using BookOrganizer.Domain;
using FluentAssertions;
using System;
using Xunit;

namespace BookOrganizer.DomainTests
{
    public class BookTests
    {
        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void WhenBookTitleIsSetNullOrEmpty_ThrowsArgumentOutOfRangeException(string name)
        {
            var book = new Book();

            Action action = () => book.Title = name;

            action.Should().Throw<ArgumentOutOfRangeException>();
        }

        [Fact]
        public void WhenTryingToSetBookTitleLongerThan256Characters_ThrowsArgumentOutOfRangeException()
        {
            var book = new Book();

            Action action = ()
                => book.Title = "Spicy jalapeno bacon ipsum dolor amet prosciutto swine andouille hamburger tri-tip ground round pork " +
                "belly. Capicola chuck andouille, short ribs turducken salami short loin filet mignon biltong pork belly fatback. " +
                "Drumstick jowl pork chop, short ribs prosciutto picanha pork landjaeger pork loin.";

            action.Should().Throw<ArgumentOutOfRangeException>();
        }

        [Theory]
        [InlineData(0)]
        [InlineData(2501)]
        public void TryingToSetReleaseYearLessThan1OrMoreThan2500_ThrowArgumentOutException(int year)
        {
            var book = new Book();
            Action action = () => book.ReleaseYear = year;

            action.Should().Throw<ArgumentOutOfRangeException>();
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        [InlineData(11_546)]
        [InlineData(10_001)]
        public void TryingToSetBookPageCountLessThanOneOrOver10000_ThrowsArgumentOutOfRangeException(int pageCount)
        {
            var book = new Book();
            Action action = () => book.PageCount = pageCount;

            action.Should().Throw<ArgumentOutOfRangeException>();
        }

        [Theory]
        [InlineData("0553103547")]
        [InlineData("000224585X")]
        [InlineData("0553106635")]
        [InlineData("0002247437")]
        [InlineData("9780553801477")]
        [InlineData("9781566199094")]
        [InlineData("9781402894626")]
        [InlineData("")]
        public void BookIsbnAcceptsStandardDefinedISBN10Or13ValueOrEmptyString(string isbn)
        {
            var book = new Book();

            book.ISBN = isbn;

            book.ISBN.Should().BeOfType(typeof(string)).And.Equals(isbn);
        }

        [Theory]
        [InlineData("9780553801477Y")]
        [InlineData("000224585Z")]
        [InlineData("ABCDEFGHI")]
        [InlineData("978055380147XX")]
        [InlineData("000224585ZRR")]
        [InlineData("ABCDEFGHIJKLM")]
        public void TryingToSetBookIsbnNonStandardOrNonEmptyValue_ThrowArgumentOutOfRangeException(string isbn)
        {
            var book = new Book();

            Action action = () => book.ISBN = isbn;

            action.Should().Throw<ArgumentOutOfRangeException>();
        }

        [Theory]
        [InlineData(-1)]
        public void TryingToSetBookWordCountNegative_ThrowsArgumentOutOfRangeException(int wordCount)
        {
            var book = new Book();
            Action action = () => book.WordCount = wordCount;

            action.Should().Throw<ArgumentOutOfRangeException>();
        }

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
