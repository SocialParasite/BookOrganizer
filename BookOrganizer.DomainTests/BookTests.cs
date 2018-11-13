using BookOrganizer.Domain;
using System;
using Xunit;
using FluentAssertions;

namespace BookOrganizer.DomainTests
{
    public class BookTests
    {
        [Fact]
        public void BookShouldBeIdentifiedByValidGuid()
        {
            var book = new Book();
            book.Id = Guid.NewGuid();
            book.Id.Should().NotBe(Guid.Empty);
        }

        [Fact]
        public void BookShouldAcceptTitlesLessThan256Characters()
        {
            var book = new Book();
            book.Title = "My Book";

            book.Title.Should().Equals("My Book");
        }

        [Fact]
        public void BookTitleShouldNotBeLongerThan256Characters()
        {
            var book = new Book();

            Action action = ()
                => book.Title = "Spicy jalapeno bacon ipsum dolor amet prosciutto swine andouille hamburger tri-tip ground round pork " +
                "belly. Capicola chuck andouille, short ribs turducken salami short loin filet mignon biltong pork belly fatback. " +
                "Drumstick jowl pork chop, short ribs prosciutto picanha pork landjaeger pork loin.";

            action.Should().Throw<ArgumentOutOfRangeException>();
        }
    }
}
