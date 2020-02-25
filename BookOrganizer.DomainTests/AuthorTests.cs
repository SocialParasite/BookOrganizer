using BookOrganizer.Domain;
using FluentAssertions;
using System;
using Xunit;

namespace BookOrganizer.DomainTests
{
    public class AuthorTests
    {
        [Fact]
        public void AuthorDateOfBirthShouldAcceptDate()
        {
            var author = new Author();
            author.DateOfBirth = new DateTime(1950, 12, 24);

            author.DateOfBirth.Should().HaveYear(1950);
            author.DateOfBirth.Should().HaveMonth(12);
            author.DateOfBirth.Should().HaveDay(24);
        }

        [Fact]
        public void AuthorDateOfBirthShouldAcceptNull()
        {
            var author = new Author();
            author.DateOfBirth = null;

            author.DateOfBirth.Should().BeNull();
        }

        [Fact]
        public void AuthorBiographyShouldAcceptStringInput()
        {
            var author = new Author();
            author.Biography = string.Empty;

            author.Biography.Should().BeOfType<string>();
            author.Biography.Should().BeEmpty();
        }

        [Fact]
        public void AuthorMugshotsAreStoredUnderCurrentUsersProfileInAuthorPicsSubfolder()
        {
            var author = new Author();

            author.MugShotPath = @"C:\temp\testingAuthorPicsPath\fake.jpg";

            var test = "C:\\Users\\tonij\\Pictures\\BookOrganizer\\AuthorPics\\fake.jpg";

            author.MugShotPath.Should().Equals(test);
        }
    }
}
