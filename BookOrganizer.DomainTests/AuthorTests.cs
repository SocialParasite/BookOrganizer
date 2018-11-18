using BookOrganizer.Domain;
using FluentAssertions;
using System;
using System.Collections.Generic;
using Xunit;

namespace BookOrganizer.DomainTests
{
    public class AuthorTests
    {
        [Fact]
        public void AuthorShouldHaveAnIdentifierOfTypeGuid()
        {
            var author = new Author();
            author.Id = Guid.NewGuid();
            Assert.IsType<Guid>(author.Id);
            author.Id.Should().NotBe(Guid.Empty);
        }

        [Fact]
        public void AuthorFirstNameShouldAcceptStringsLessThan50Characters()
        {
            var author = new Author();
            author.FirstName = "Patrick";

            author.FirstName.Should().NotBeEmpty().And.NotBeNull().Equals("Patrick");
        }

        [Fact]
        public void AuthorLastNameShouldAcceptStringsLessThan50Characters()
        {
            var author = new Author();
            author.LastName = "Rothfuss";

            author.LastName.Should().NotBeEmpty().And.NotBeNull().Equals("Rothfuss");
        }

        [Theory]
        [InlineData("")]
        [InlineData("Nicolaus Authoritus Maximus Nicolaus Authoritus Maximus")]
        public void GivenFirstNameLessThan1OrOver50Characters_ThrowsArgumentOutOfRangeException(string firstName)
        {
            var author = new Author();
            Action action = () => author.FirstName = firstName;

            action.Should().Throw<ArgumentOutOfRangeException>();
        }

        [Theory]
        [InlineData("")]
        [InlineData("Nicolaus Authoritus Maximus Nicolaus Authoritus Maximus")]
        public void GivenLastNameLessThan1OrOver50Characters_ThrowsArgumentOutOfRangeException(string lastName)
        {
            var author = new Author();
            Action action = () => author.LastName = lastName;

            action.Should().Throw<ArgumentOutOfRangeException>();
        }

        [Fact]
        public void AuthorShouldHaveADateOfBirthProperty()
        {
            var author = new Author();
            author.DateOfBirth = new DateTime(1990, 12, 24);

            author.DateOfBirth.Should().HaveDay(24).And.HaveMonth(12).And.HaveYear(1990);
        }

        public static List<BookAuthors> BookTestCases()
        {
            var author = new Author();
            var books = new List<Book> {
                new Book { Title = "Awesome Debut" },
                new Book { Title = "Buy this book" },
                new Book { Title = "Steal this one" }
            };

            var bookAuthors = new List<BookAuthors> {
                new BookAuthors { Id = Guid.NewGuid(), Book = books[0], Author = author },
                new BookAuthors { Id = Guid.NewGuid(), Book = books[1], Author = author },
                new BookAuthors { Id = Guid.NewGuid(), Book = books[2], Author = author },
            };

            return bookAuthors;
        }

        [Fact]
        public void AuthorMayHaveWrittenSeveralBooks()
        {
            var author = new Author();
            author.BooksLink = BookTestCases();

            author.BooksLink.Should().NotBeEmpty();
            author.BooksLink.Should().HaveCount(3);
        }

        [Fact]
        public void NationalityShouldHoldAuthorsCountryAsOfTypeNationality()
        {
            var author = new Author();
            author.Nationality = new Nationality();

            author.Nationality.Should().BeOfType<Nationality>();
        }
    }
}
