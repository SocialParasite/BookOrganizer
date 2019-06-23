using BookOrganizer.Domain;
using FluentAssertions;
using System;
using Xunit;

namespace BookOrganizer.DomainTests
{
    public class NationalityTests
    {
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
        public void Name_ShouldRaise_PropertyChangedEvent()
        {
            var nationality = new Nationality();
            var raised = false;

            nationality.PropertyChanged += (Sender, e) =>
            {
                raised = true;
            };

            nationality.Name = "canadian";

            raised.Should().BeTrue();
        }

    }
}
