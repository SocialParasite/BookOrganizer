using BookOrganizer.Domain;
using System;
using Xunit;
using FluentAssertions;
using System.IO;

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
        public void WhenBookNameIsSetNullOrEmptyThrowsArgumentException(string name)
        {
            var book = new Book();

            Action action = () => book.Title = name;

            action.Should().Throw<ArgumentOutOfRangeException>();
        }

        [Fact]
        public void BookTitleShouldNotBeLongerThan256Characters()
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
        public void ReleaseYearNotWithin1And2500ShouldThrowArgumentException(int year)
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
        public void BookIsbnDoesNotAcceptNonStandardNonEmptyValue(string isbn)
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
    }
}
