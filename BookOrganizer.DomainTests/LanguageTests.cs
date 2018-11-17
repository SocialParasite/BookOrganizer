using BookOrganizer.Domain;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace BookOrganizer.DomainTests
{
    public class LanguageTests
    {
        [Fact]
        public void LanguageShouldBeIdentifiedByValidGuid()
        {
            var language = new Language();
            language.Id = Guid.NewGuid();

            language.Id.Should().NotBe(Guid.Empty);
        }

        [Fact]
        public void LanguageShouldAcceptNamesLessThan32Characters()
        {
            var language = new Language();
            language.LanguageName = "Rallydriver english";

            language.LanguageName.Should().BeOfType(typeof(string)).And.Equals("Rallydriver english");
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void WhenLanguageNameIsSetNullOrEmptyThrowsArgumentException(string name)
        {
            var language = new Language();

            Action action = () => language.LanguageName = name;

            action.Should().Throw<ArgumentOutOfRangeException>();
        }

        [Fact]
        public void TryingToSetLanguageNameLongerThan32CharactersShouldThrowArgumentOutOfRangeException()
        {
            var language = new Language();

            Action action = ()
                => language.LanguageName = "Spicy jalapeno bacon ipsum dolor amet.";// prosciutto swine andouille hamburger tri-tip ground round pork";

            action.Should().Throw<ArgumentOutOfRangeException>();
        }
    }
}
