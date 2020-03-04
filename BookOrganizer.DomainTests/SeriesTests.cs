using BookOrganizer.Domain;
using FluentAssertions;
using System;
using Xunit;

namespace BookOrganizer.DomainTests
{
    public class SeriesTests
    {
        [Fact]
        public void SeriesPicturesAreStoredUnderCurrentUsersProfileInSeriesPicturesSubfolder()
        {
            var series = new Series();

            series.PicturePath = @"C:\temp\testingBookCoverPath\fake.jpg";

            var test = "C:\\Users\\tonij\\Pictures\\BookOrganizer\\SeriesPictures\\fake.jpg";

            series.PicturePath.Should().Equals(test);
        }

        [Fact]
        public void SeriesDescriptionShouldAcceptStringInput()
        {
            var series = new Series();
            series.Description = string.Empty;

            series.Description.Should().BeOfType<string>();
            series.Description.Should().BeEmpty();
        }
    }
}
