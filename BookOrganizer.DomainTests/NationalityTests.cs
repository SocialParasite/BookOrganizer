using BookOrganizer.Domain;
using FluentAssertions;
using System;
using System.Collections.Generic;
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
        public void TryingToSetNationalityNameToNullOrEmpty_ThrowsArgumentOutOfRangeException(string name)
        {
            var nationality = new Nationality();

            Action action = () => nationality.Name = name;

            action.Should().Throw<ArgumentOutOfRangeException>();
        }

        [Fact]
        public void TryingToSetNationalityNameLongerThan32Characters_ThrowsArgumentOutOfRangeException()
        {
            var nationality = new Nationality();

            Action action = ()
                => nationality.Name = "Spicy jalapeno bacon ipsum dolor amet prosciutto swine andouille hamburger tri-tip ground round pork";

            action.Should().Throw<ArgumentOutOfRangeException>();
        }

        [Fact]
        public void NationalityShouldHaveAListOfAuthorsBornInThatCountry()
        {
            var nationality = new Nationality();
            nationality.Authors = new List<Author> { new Author { LastName = "Rothfuss" }, new Author { LastName = "Martin" } };
            nationality.Authors.Should().HaveCount(2);
        }
    }
}
