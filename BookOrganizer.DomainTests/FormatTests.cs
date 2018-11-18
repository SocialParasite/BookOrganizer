using BookOrganizer.Domain;
using FluentAssertions;
using System;
using System.Collections.Generic;
using Xunit;

namespace BookOrganizer.DomainTests
{
    public class FormatTests
    {
        [Fact]
        public void FormatShouldBeIdentifiedByValidGuid()
        {
            var format = new Format();
            format.Id = Guid.NewGuid();

            format.Id.Should().NotBe(Guid.Empty);
        }

        [Fact]
        public void FormatShouldAcceptNamesLessThan32Characters()
        {
            var format = new Format();
            format.Name = "Portable Document Format";

            format.Name.Should().BeOfType(typeof(string)).And.Equals("Portable Document Format");
        }

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


        [Fact]
        public void FormatAbbreveationShouldAcceptNamesLessThan16Characters()
        {
            var format = new Format();
            format.Abbreveation = "pdf";

            format.Abbreveation.Should().BeOfType(typeof(string)).And.Equals("pdf");
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

        [Fact]
        public void FormatShouldHaveReferenceToBooksOwnedInThatFormat()
        {
            var format = new Format();
            format.BookLink = new List<BooksFormats>
            {
                new BooksFormats { Format = format, Book = new Book { Title = "Game Of Thrones" } },
                new BooksFormats { Format = format, Book = new Book { Title = "Clash of Kings" } }
            };

            format.BookLink.Should().HaveCount(2);
        }
    }
}
