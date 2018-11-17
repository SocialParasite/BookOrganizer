using BookOrganizer.Domain;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace BookOrganizer.DomainTests
{
    public class FormatTests
    {
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
        public void WhenFormatNameIsSetNullOrEmptyThrowsArgumentException(string name)
        {
            var format = new Format();

            Action action = () => format.Name = name;

            action.Should().Throw<ArgumentOutOfRangeException>();
        }

        [Fact]
        public void FormatNameShouldNotBeLongerThan32Characters()
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
        public void WhenFormatAbbreveationIsSetNullOrEmptyThrowsArgumentException(string abbreveation)
        {
            var format = new Format();

            Action action = () => format.Abbreveation = abbreveation;

            action.Should().Throw<ArgumentOutOfRangeException>();
        }

        [Fact]
        public void FormatAbbreveationShouldNotBeLongerThan16Characters()
        {
            var format = new Format();

            Action action = ()
                => format.Abbreveation = "portableformatdoc";

            action.Should().Throw<ArgumentOutOfRangeException>();
        }
    }
}
