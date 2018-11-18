using BookOrganizer.Domain;
using FluentAssertions;
using System;
using System.Collections.Generic;
using Xunit;

namespace BookOrganizer.DomainTests
{
    public class GenreTests
    {
        [Fact]
        public void GenreShouldBeIdentifiedByValidGuid()
        {
            var genre = new Genre();
            genre.Id = Guid.NewGuid();

            genre.Id.Should().NotBe(Guid.Empty);
        }

        [Fact]
        public void BookNameShouldAcceptStringsAsInput()
        {
            var genre = new Genre();

            genre.Name = "Horror";

            genre.Name.Should().Equals("Horror");
        }

        [Fact]
        public void GenreShouldAcceptNamesLessThan32Characters()
        {
            var genre = new Genre();
            genre.Name = "Horror";

            genre.Name.Should().BeOfType(typeof(string)).And.Equals("Horror");
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void WhenGenreNameIsSetNullOrEmpty_ThrowsArgumentOutOfRangeException(string name)
        {
            var genre = new Genre();

            Action action = () => genre.Name = name;

            action.Should().Throw<ArgumentOutOfRangeException>();
        }

        [Fact]
        public void TryingToSetGenreNameLongerThan32Characters_ThrowsArgumentOutOfRangeException()
        {
            var genre = new Genre();

            Action action = ()
                => genre.Name = "Spicy jalapeno bacon ipsum dolor amet prosciutto swine andouille hamburger tri-tip ground round pork";

            action.Should().Throw<ArgumentOutOfRangeException>();
        }

        [Fact]
        public void GenreShouldHaveReferenceToBooksBelongingUnderIt()
        {
            var genre = new Genre();
            genre.BookLink = new List<BookGenres>
            {
                new BookGenres { Genre = genre, Book = new Book { Title = "Slow Regards of Silent Things" } },
                new BookGenres { Genre = genre, Book = new Book { Title = "Queen of Fire" } }
            };

            genre.BookLink.Should().HaveCount(2);
        }
    }
}
