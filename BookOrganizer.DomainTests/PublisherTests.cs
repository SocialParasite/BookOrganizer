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


        [Fact]
        public void PublisherName_ShouldRaise_PropertyChangedEvent()
        {
            var publisher = new Publisher();
            var raised = false;

            publisher.PropertyChanged += (Sender, e) =>
            {
                raised = true;
            };

            publisher.Name = "Storytellers Inc.";

            raised.Should().BeTrue();
        }


        [Fact]
        public void LogoPath_ShouldRaise_PropertyChangedEvent()
        {
            var publisher = new Publisher();
            var raised = false;

            publisher.PropertyChanged += (Sender, e) =>
            {
                raised = true;
            };

            publisher.LogoPath = @"C:\temp";

            raised.Should().BeTrue();
        }

        [Fact]
        public void Description_ShouldRaise_PropertyChangedEvent()
        {
            var publisher = new Publisher();
            var raised = false;

            publisher.PropertyChanged += (Sender, e) =>
            {
                raised = true;
            };

            publisher.Description = "Storytellers Inc. was established because the name happened to be unregistered at a time.";

            raised.Should().BeTrue();
        }

    }
}
