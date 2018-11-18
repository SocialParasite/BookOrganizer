using BookOrganizer.Domain;
using FluentAssertions;
using System;
using System.Collections.Generic;
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
        public void PublisherShouldHaveListOfBooksItHasPublished()
        {
            var publisher = new Publisher();
            publisher.Books = new List<Book> { new Book { Title = "Altered Carbon" }, new Book { Title = "Broken Angels" } };

            publisher.Books.Should().HaveCount(2);
        }
    }
}
