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
        public void WhenBookHaveBeenReadTheDateShouldBeStored()
        {
            var booksReadDate = new BooksReadDate();
            booksReadDate.ReadDate = DateTime.Today;

            booksReadDate.ReadDate.Should().Be(DateTime.Today);
        }
    }
}
