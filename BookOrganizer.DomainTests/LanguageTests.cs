using BookOrganizer.Domain;
using FluentAssertions;
using System;
using Xunit;

namespace BookOrganizer.DomainTests
{
    public class LanguageTests
    {
        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void WhenLanguageNameIsSetNullOrEmpty_ThrowsArgumentOutOfRangeException(string name)
        {
            var language = new Language();

            Action action = () => language.LanguageName = name;

            action.Should().Throw<ArgumentOutOfRangeException>();
        }

        [Fact]
        public void TryingToSetLanguageNameLongerThan32Characters_ThrowsArgumentOutOfRangeException()
        {
            var language = new Language();

            Action action = ()
                => language.LanguageName = "Spicy jalapeno bacon ipsum dolor amet.";

            action.Should().Throw<ArgumentOutOfRangeException>();
        }
    }
}
