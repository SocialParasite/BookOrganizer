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

        [Fact]
        public void PublisherDescriptionShouldAcceptStringInput()
        {
            var publisher = new Publisher();
            publisher.Description = string.Empty;

            publisher.Description.Should().BeOfType<string>();
            publisher.Description.Should().BeEmpty();
        }


        [Fact]
        public void PublisherLogosAreStoredUnderCurrentUsersProfileInPublisherLogosSubfolder()
        {
            var publisher = new Publisher();

            publisher.LogoPath = @"C:\temp\testingAuthorPicsPath\fake.jpg";

            var test = "C:\\Users\\tonij\\Pictures\\BookOrganizer\\PublisherLogos\\fake.jpg";

            publisher.LogoPath.Should().Equals(test);
        }

    }
}
