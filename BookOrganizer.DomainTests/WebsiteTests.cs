using BookOrganizer.Domain;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace BookOrganizer.DomainTests
{
    public class WebsiteTests
    {
        [Fact]
        public void WebsiteShouldBeIdentifiedByValidGuid()
        {
            var website = new Website();
            website.Id = Guid.NewGuid();

            website.Id.Should().NotBe(Guid.Empty);
        }

        [Fact]
        public void WebsiteShouldAcceptURIsLessThan256Characters()
        {
            var website = new Website();
            website.URI = "http://www.georgerrmartin.com/";

            website.URI.Should().BeOfType(typeof(string)).And.Equals("http://www.georgerrmartin.com/");
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void WhenLWebsiteURIIsSetNullOrEmptyThrowsArgumentException(string uri)
        {
            var website = new Website();

            Action action = () => website.URI = uri;

            action.Should().Throw<ArgumentOutOfRangeException>();
        }

        [Fact]
        public void TryingToSetWebsiteURILongerThan256CharactersShouldThrowArgumentOutOfRangeException()
        {
            var website = new Website();

            Action action = ()
                => website.URI = "Spicy jalapeno bacon ipsum dolor amet prosciutto swine andouille hamburger tri-tip ground round pork. " +
                "Spicy jalapeno bacon ipsum dolor amet prosciutto swine andouille hamburger tri-tip ground round pork. " +
                "Spicy jalapeno bacon ipsum dolor amet prosciutto swine andouille hamburger tri-tip ground round pork";

            action.Should().Throw<ArgumentOutOfRangeException>();
        }
    }
}
