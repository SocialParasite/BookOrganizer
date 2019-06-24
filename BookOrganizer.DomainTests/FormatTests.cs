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

        [Fact]
        public void Name_ShouldRaise_PropertyChangedEvent()
        {
            var format = new Format();
            var raised = false;

            format.PropertyChanged += (Sender, e) =>
            {
                raised = true;
            };

            format.Name = "pdf";

            raised.Should().BeTrue();
        }

    }
}
