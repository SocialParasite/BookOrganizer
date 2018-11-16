using BookOrganizer.Domain;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace BookOrganizer.DomainTests
{
    public class GenreTests
    {
        [Fact]
        public void BookMayBelongToDifferentGenres()
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
        public void WhenGenreNameIsSetNullOrEmptyThrowsArgumentException(string name)
        {
            var genre = new Genre();

            Action action = () => genre.Name = name;

            action.Should().Throw<ArgumentOutOfRangeException>();
        }

        [Fact]
        public void GenreNameShouldNotBeLongerThan32Characters()
        {
            var genre = new Genre();

            Action action = ()
                => genre.Name = "Spicy jalapeno bacon ipsum dolor amet prosciutto swine andouille hamburger tri-tip ground round pork";

            action.Should().Throw<ArgumentOutOfRangeException>();
        }
    }
}
