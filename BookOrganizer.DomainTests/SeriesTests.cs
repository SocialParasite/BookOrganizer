using BookOrganizer.Domain;
using FluentAssertions;
using System;
using Xunit;

namespace BookOrganizer.DomainTests
{
    public class SeriesTests
    {
        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void WhenSeriesNameIsSetNullOrEmpty_ThrowsArgumentOutOfRangeException(string name)
        {
            var series = new Series();

            Action action = () => series.Name = name;

            action.Should().Throw<ArgumentOutOfRangeException>();
        }

        [Fact]
        public void WhenTryingToSetSeriesNameLongerThan256Characters_ThrowsArgumentOutOfRangeException()
        {
            var series = new Series();

            Action action = ()
                => series.Name = "Spicy jalapeno bacon ipsum dolor amet prosciutto swine andouille hamburger tri-tip ground round pork " +
                "belly. Capicola chuck andouille, short ribs turducken salami short loin filet mignon biltong pork belly fatback. " +
                "Drumstick jowl pork chop, short ribs prosciutto picanha pork landjaeger pork loin.";

            action.Should().Throw<ArgumentOutOfRangeException>();
        }


        [Theory]
        [InlineData(0)]
        [InlineData(-10)]
        public void TryingToAddNegativeOrZeroAmountOfBooksInTheSeries_ThrowsArgumentOutOfRangeException(int amount)
        {
            var series = new Series();

            Action action = () => series.NumberOfBooks = amount;
            action.Should().Throw<ArgumentOutOfRangeException>();
        }

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
