using BookOrganizer.Domain;
using FluentAssertions;
using System;
using Xunit;

namespace BookOrganizer.DomainTests
{
    public class PublisherTests
    {
        [Fact]
        public void PublisherShouldBeIdentifiableByGuid()
        {
            var publisher = new Publisher();
            publisher.Id = Guid.NewGuid();

            publisher.Id.Should().NotBe(Guid.Empty);
        }

        [Fact]
        public void PublisherShouldAcceptNamesLessThan256Characters()
        {
            var publisher = new Publisher();
            publisher.Name = "Voyager";

            publisher.Name.Should().BeOfType(typeof(string)).And.Equals("Voyager");
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void WhenPublisherNameIsSetNullOrEmptyThrowsArgumentException(string name)
        {
            var publisher = new Publisher();

            Action action = () => publisher.Name = name;

            action.Should().Throw<ArgumentOutOfRangeException>();
        }

        [Fact]
        public void PublisherNameShouldNotBeLongerThan64Characters()
        {
            var publisher = new Publisher();

            Action action = ()
                => publisher.Name = "Spicy jalapeno bacon ipsum dolor amet prosciutto swine andouille hamburger tri-tip ground round pork";

            action.Should().Throw<ArgumentOutOfRangeException>();
        }
    }
}
