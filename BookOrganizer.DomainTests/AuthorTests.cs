using BookOrganizer.Domain;
using FluentAssertions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace BookOrganizer.DomainTests
{
    public class AuthorTests
    {
        [Fact]
        public void AuthorShouldBeIdentifiedByValidGuid()
        {
            var author = new Author();
            author.Id = Guid.NewGuid();

            author.Id.Should().NotBe(Guid.Empty);
        }

        [Fact]
        public void AuthorShouldHaveFirstName()
        {
            var author = new Author();
            author.FirstName = "Patrick";

            author.FirstName.Should().NotBeEmpty().And.NotBeNull().Equals("Patrick");
        }

        [Fact]
        public void AuthorShouldHaveALastName()
        {
            var author = new Author();
            author.LastName = "Rothfuss";

            author.LastName.Should().NotBeEmpty().And.NotBeNull().Equals("Rothfuss");
        }

        [Theory]
        [InlineData("")]
        [InlineData("Nicolaus Authoritus Maximus Nicolaus Authoritus Maximus")]
        public void ArgumentOutOfRangeExceptionShouldBeThrownIfCustomersFirstNameIsLessThan1OrMoreThan50CharactersLong(string firstName)
        {
            var author = new Author();
            Action action = () => author.FirstName = firstName;

            action.Should().Throw<ArgumentOutOfRangeException>();
        }

        [Theory]
        [InlineData("")]
        [InlineData("Nicolaus Authoritus Maximus Nicolaus Authoritus Maximus")]
        public void ArgumentOutOfRangeExceptionShouldBeThrownIfCustomersLastNameIsLessThan1OrMoreThan50CharactersLong(string lastName)
        {
            var author = new Author();
            Action action = () => author.LastName = lastName;

            action.Should().Throw<ArgumentOutOfRangeException>();
        }


        //public static IEnumerable<object[]> BookTestCases()
        //{
        //    yield return new object[] { new Book { Title = "Awesome Debut" } };
        //    yield return new object[] { new Book { Title = "Buy this book" } };
        //    yield return new object[] { new Book { Title = "Steal this one" } };
        //}

        //[Theory]
        //[MemberData(nameof(BookTestCases))]
        //public void AuthorMayHaveWrittenSeveralBooks(Book book)
        //{
        //    var author = new Author();
        //    author.Books = new List<Book>() { book };

        //    author.Books.Should().NotBeEmpty();
        //}
    }
}
