using BookOrganizer.Domain;
using FluentAssertions;
using System;
using Xunit;

namespace BookOrganizer.DomainTests
{
    public class PublisherTests
    {
        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void WhenPublisherNameIsSetNullOrEmpty_ThrowsArgumentOutOfRangeException(string name)
        {
            var publisher = new Publisher();

            Action action = () => publisher.Name = name;

            action.Should().Throw<ArgumentOutOfRangeException>();
        }

        [Fact]
        public void TryingToSetPublisherNameLongerThan64Characters_ThrowsArgumentOutOfRangeException()
        {
            var publisher = new Publisher();

            Action action = ()
                => publisher.Name = "Spicy jalapeno bacon ipsum dolor amet prosciutto swine andouille hamburger tri-tip ground round pork";

            action.Should().Throw<ArgumentOutOfRangeException>();
        }
    }
}
