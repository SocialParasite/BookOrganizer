using BookOrganizer.Domain;
using System;
using Xunit;

namespace BookOrganizer.DomainTests
{
    public class BookTests
    {
        [Fact]
        public void BookShouldBeIdentifiedByGuid()
        {
            var book = new Book();
            book.Id = Guid.NewGuid();

            Assert.NotEqual<Guid>(Guid.Empty, book.Id);
        }
    }
}
