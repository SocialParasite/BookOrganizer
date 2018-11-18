using BookOrganizer.Domain;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.IO;
using Xunit;

namespace BookOrganizer.DomainTests
{
    public class BookTests
    {
        [Fact]
        public void BookShouldBeIdentifiedByValidGuid()
        {
            var book = new Book();
            book.Id = Guid.NewGuid();

            book.Id.Should().NotBe(Guid.Empty);
        }

        [Fact]
        public void BookShouldAcceptTitlesLessThan256Characters()
        {
            var book = new Book();
            book.Title = "My Book";

            book.Title.Should().BeOfType(typeof(string)).And.Equals("My Book");
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void WhenBookNameIsSetNullOrEmpty_ThrowsArgumentOutOfRangeException(string name)
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

        [Fact]
        public void BookShouldHaveAReleaseYearOfTypeInt()
        {
            var book = new Book();
            book.ReleaseYear = 2018;

            book.ReleaseYear.Should().BeOfType(typeof(int));
        }

        [Theory]
        [InlineData(2000)]
        [InlineData(1)]
        [InlineData(2500)]
        public void ReleaseYearShouldAcceptValuesBetween0And2500(int year)
        {
            var book = new Book();
            book.ReleaseYear = year;
            book.ReleaseYear.Should().Equals(year);
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
        [InlineData(1)]
        [InlineData(100)]
        [InlineData(1154)]
        [InlineData(2141)]
        [InlineData(10_000)]
        public void BookShouldHaveAPageCountAbove1(int pageCount)
        {
            var book = new Book();
            book.PageCount = pageCount;

            book.PageCount.Should().BeOfType(typeof(int)).And.BePositive();
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

        [Fact]
        public void IfBookIsReadIsReadShouldReturnTrue()
        {
            var book = new Book();
            book.IsRead = true;

            book.IsRead.Should().BeTrue();
        }

        [Fact]
        public void ByDefaultIsReadShouldReturnFalse()
        {
            var book = new Book();

            book.IsRead.Should().BeFalse();
        }

        [Fact]
        public void BookShouldHaveAOptionalDescriptionField()
        {
            var book = new Book();

            book.Description = "This is an awesome book!";

            book.Description.Should().BeOfType(typeof(string));
        }

        [Theory]
        [InlineData("Cover1.jpg")]
        [InlineData(@"ISaveWhere\I\Want\WhatIWant.bmp")]
        [InlineData(@"U...can\tPredicate\what\i\doooo\nimda.exe.jpg")]
        public void BookCoverPicturePathShouldBeStoredInPicturesFolderInInstallationLocation(string pic)
        {
            var book = new Book();
            var picFileName = Path.GetFileName(pic);
            var envPath = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
            var path = @"\BookwormBookOrganizer\Covers\";
            path = $"{envPath}{path}{picFileName}";
            book.BookCoverPicture = pic;

            book.BookCoverPicture.Should().Equals(path);
        }

        [Fact]
        public void IfBookHaveMoreThanOneAuthorItCanStoreThemAll()
        {

            var book = new Book();

            book.AuthorsLink = new List<BookAuthors>
            {

                new BookAuthors { Author = new Author { FirstName = "Patric", LastName = "Rothfuss" } },
                new BookAuthors { Author = new Author { FirstName = "George R.R.", LastName = "Martin" } },
                new BookAuthors { Author = new Author { FirstName = "Scott", LastName = "Lynch" } }
            };

            book.AuthorsLink.Should().NotBeEmpty();
            book.AuthorsLink.Should().HaveCount(3);
        }


        [Fact]
        public void BookShouldStoreReferenceToLanguageItsWrittenIn()
        {
            var book = new Book();
            book.Language = new Language();
            book.Language.Should().BeOfType<Language>();
        }

        [Fact]
        public void BookShouldStoreReferenceToItsPublisher()
        {
            var book = new Book();
            book.Publisher = new Publisher();
            book.Publisher.Should().BeOfType<Publisher>();
        }

        [Fact]
        public void BookShouldHaveReferenceToDatesItWasRead()
        {
            var book = new Book();
            book.ReadDates = new List<BooksReadDate>
            {
                new BooksReadDate { Book = book, ReadDate = DateTime.Now.AddYears(-1) },
                new BooksReadDate { Book = book, ReadDate = DateTime.Today}
            };

            book.ReadDates.Should().NotBeEmpty().And.HaveCount(2);
        }

        [Fact]
        public void BookShouldHaveReferenceToGenresItBelongsTo()
        {
            var book = new Book();
            book.GenreLink = new List<BookGenres>
            {
                new BookGenres { Book = book, Genre = new Genre { Name = "Horror" } },
                new BookGenres { Book = book, Genre = new Genre { Name = "Sci-fi" } }
            };

            book.GenreLink.Should().HaveCount(2);
        }

        [Fact]
        public void BookShouldHaveReferenceToFormatsItsOwned()
        {
            var book = new Book();
            book.FormatLink = new List<BooksFormats>
            {
                new BooksFormats { Book = book, Format = new Format { Abbreveation = "pdf", Name = "Portable Document Format" } },
                new BooksFormats { Book = book, Format = new Format { Abbreveation = "asw", Name = "Kindle File Format" } }
            };

            book.FormatLink.Should().HaveCount(2);
        }

        [Fact]
        public void BookShouldStoreReferenceToSeriesItsPartOf()
        {
            var book = new Book();
            book.BookSeries = new Series();
            book.BookSeries.Should().BeOfType<Series>();
        }
    }
}
