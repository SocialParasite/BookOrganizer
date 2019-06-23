using BookOrganizer.Domain;
using FluentAssertions;
using System;
using Xunit;

namespace BookOrganizer.DomainTests
{
    public class AuthorTests
    {
        [Theory]
        [InlineData("")]
        [InlineData("Nicolaus Authoritus Maximus Nicolaus Authoritus Maximus")]
        public void GivenFirstNameLessThan1OrOver50Characters_ThrowsArgumentOutOfRangeException(string firstName)
        {
            var author = new Author();
            Action action = () => author.FirstName = firstName;

            action.Should().Throw<ArgumentOutOfRangeException>();
        }

        [Theory]
        [InlineData("")]
        [InlineData("Nicolaus Authoritus Maximus Nicolaus Authoritus Maximus")]
        public void GivenLastNameLessThan1OrOver50Characters_ThrowsArgumentOutOfRangeException(string lastName)
        {
            var author = new Author();
            Action action = () => author.LastName = lastName;

            action.Should().Throw<ArgumentOutOfRangeException>();
        }

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

        [Fact]
        public void AuthorFirstName_ShouldRaise_PropertyChangedEvent()
        {
            var author = new Author();
            var raised = false;

            author.PropertyChanged += (Sender, e) =>
            {
                raised = true;
            };

            author.FirstName = "Peter";

            raised.Should().BeTrue();
        }

        [Fact]
        public void AuthorLastName_ShouldRaise_PropertyChangedEvent()
        {
            var author = new Author();
            var raised = false;

            author.PropertyChanged += (Sender, e) =>
            {
                raised = true;
            };

            author.LastName = "Peterson";

            raised.Should().BeTrue();
        }
        [Fact]
        public void DateOfBirth_ShouldRaise_PropertyChangedEvent()
        {
            var author = new Author();
            var raised = false;

            author.PropertyChanged += (Sender, e) =>
            {
                raised = true;
            };

            author.DateOfBirth = DateTime.Today;

            raised.Should().BeTrue();
        }
        [Fact]
        public void MugShotPath_ShouldRaise_PropertyChangedEvent()
        {
            var author = new Author();
            var raised = false;

            author.PropertyChanged += (Sender, e) =>
            {
                raised = true;
            };

            author.MugShotPath = @"C:\temp";

            raised.Should().BeTrue();
        }

        [Fact]
        public void Biography_ShouldRaise_PropertyChangedEvent()
        {
            var author = new Author();
            var raised = false;

            author.PropertyChanged += (Sender, e) =>
            {
                raised = true;
            };

            author.Biography = "Once upon a time child was born. He was named Peterson, Peter Peterson.";

            raised.Should().BeTrue();
        }
    }
}
