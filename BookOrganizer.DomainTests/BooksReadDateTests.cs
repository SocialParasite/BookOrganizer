using BookOrganizer.Domain;
using FluentAssertions;
using System;
using Xunit;

namespace BookOrganizer.DomainTests
{
    public class BooksReadDateTests
    {
        [Fact]
        public void BooksReadDateShouldAcceptDate()
        {
            var booksReadDate = new BooksReadDate();
            booksReadDate.ReadDate = new DateTime(1950, 12, 24);

            booksReadDate.ReadDate.Should().HaveYear(1950);
            booksReadDate.ReadDate.Should().HaveMonth(12);
            booksReadDate.ReadDate.Should().HaveDay(24);
        }
    }
}
