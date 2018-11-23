using BookOrganizer.Domain;
using FluentAssertions;
using System;
using Xunit;

namespace BookOrganizer.DomainTests
{
    public class GenreTests
    {
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
    }
}
