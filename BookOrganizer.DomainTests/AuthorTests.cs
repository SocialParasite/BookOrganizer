using BookOrganizer.Domain;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace BookOrganizer.DomainTests
{
    public class AuthorTests
    {
        [Fact]
        public void AuthorShouldBeIdentifiedByValidGuid()
        {
            var author = new Author();
            author.Id = Guid.NewGuid();

            author.Id.Should().NotBe(Guid.Empty);
        }
    }
}
