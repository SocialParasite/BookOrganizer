using BookOrganizer.Domain;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace BookOrganizer.DomainTests
{
    public class BooksReadDateTests
    {
        [Fact]
        public void BooksReadDateShouldHaveAnIdentifierOfTypeGuid()
        {
            var booksReadDate = new BooksReadDate();
            booksReadDate.Id = Guid.NewGuid();

            Assert.IsType<Guid>(booksReadDate.Id);
            booksReadDate.Id.Should().NotBe(Guid.Empty);
        }

        [Fact]
        public void WhenBookHaveBeenReadTheDateShouldBeStored()
        {
            var booksReadDate = new BooksReadDate();
            booksReadDate.ReadDate = DateTime.Today;

            booksReadDate.ReadDate.Should().Be(DateTime.Today);
        }

        [Fact]
        public void ReadDateShouldBeAssociatedWithABook()
        {
            var booksReadDate = new BooksReadDate();
            booksReadDate.Book = new Book();

            booksReadDate.Book.Should().BeOfType<Book>();
        }
    }
}
