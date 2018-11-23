using BookOrganizer.Domain;
using FluentAssertions;
using System;
using Xunit;

namespace BookOrganizer.DomainTests
{
    public class FormatTests
    {
        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void WhenFormatNameIsSetNullOrEmpty_ThrowsArgumentOutOfRangeException(string name)
        {
            var format = new Format();

            Action action = () => format.Name = name;

            action.Should().Throw<ArgumentOutOfRangeException>();
        }

        [Fact]
        public void TryingToSetFormatNameLongerThan32Characters_ThrowsArgumentOutOfRangeException()
        {
            var format = new Format();

            Action action = ()
                => format.Name = "Spicy jalapeno bacon ipsum dolor amet prosciutto swine andouille hamburger tri-tip ground round pork";

            action.Should().Throw<ArgumentOutOfRangeException>();
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void WhenFormatAbbreveationIsSetNullOrEmpty_ThrowsArgumentOutOfException(string abbreveation)
        {
            var format = new Format();

            Action action = () => format.Abbreveation = abbreveation;

            action.Should().Throw<ArgumentOutOfRangeException>();
        }

        [Fact]
        public void TryingToSetFormatAbbreveationLongerThan16Characters_ThrowsArgumentOutOfRangeException()
        {
            var format = new Format();

            Action action = ()
                => format.Abbreveation = "portableformatdoc";

            action.Should().Throw<ArgumentOutOfRangeException>();
        }
    }
}
