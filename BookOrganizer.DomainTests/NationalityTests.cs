using BookOrganizer.Domain;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace BookOrganizer.DomainTests
{
    public class NationalityTests
    {
        [Fact]
        public void NationalityShouldBeIdentifiedByValidGuid()
        {
            var nationality = new Nationality();
            nationality.Id = Guid.NewGuid();

            nationality.Id.Should().NotBe(Guid.Empty);
        }

        [Fact]
        public void NationalityShouldAcceptNamesLessThan32Characters()
        {
            var nationality = new Nationality();
            nationality.Name = "USA";

            nationality.Name.Should().BeOfType(typeof(string)).And.Equals("USA");
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void WhenNationalityNameIsSetNullOrEmptyThrowsArgumentException(string name)
        {
            var nationality = new Nationality();

            Action action = () => nationality.Name = name;

            action.Should().Throw<ArgumentOutOfRangeException>();
        }

        [Fact]
        public void TryingToSetNationalityNameLongerThan32CharactersShouldThrowArgumentOutOfRangeException()
        {
            var nationality = new Nationality();

            Action action = ()
                => nationality.Name = "Spicy jalapeno bacon ipsum dolor amet prosciutto swine andouille hamburger tri-tip ground round pork";

            action.Should().Throw<ArgumentOutOfRangeException>();
        }
    }
}
