using BookOrganizer.Domain;
using FluentAssertions;
using Xunit;

namespace BookOrganizer.DomainTests
{
    public class PublisherTests
    {
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
