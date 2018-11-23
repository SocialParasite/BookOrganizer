using BookOrganizer.Domain;
using FluentAssertions;
using System;
using Xunit;

namespace BookOrganizer.DomainTests
{
    public class AuthorTests
    {
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
    }
}
