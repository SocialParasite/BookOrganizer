using BookOrganizer.Domain;
using FluentAssertions;
using System;
using System.Collections.Generic;
using Xunit;

namespace BookOrganizer.DomainTests
{
    public class SeriesTests
    {
        [Fact]
        public void NewSeriesCanBeAddedIntoSystem()
        {
            var series = new Series();
            series.Name = "Song of Ice and Fire";

            series.Name.Should().Equals("Song of Ice and Fire");
        }

        [Fact]
        public void BookSeriesShouldBeIdentifiedByGuid()
        {
            var series = new Series();

            series.Id = Guid.NewGuid();

            series.Id.Should().NotBe(Guid.Empty);
        }

        [Fact]
        public void BookSeriesShouldKnowTheAmountOfBooksInTheSeries()
        {
            var series = new Series();
            series.NumberOfBooks = 3;

            series.NumberOfBooks.Should().BeInRange(2, 1000);
        }

        [Theory]
        [InlineData(42)]
        [InlineData(1)]
        public void AmountOfBooksInTheSeriesShouldBePositiveNumberOverZero(int amount)
        {
            var series = new Series();
            series.NumberOfBooks = amount;

            series.NumberOfBooks.Should().BeInRange(1, 1000);
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
        public void BooksSeriesShouldHaveACollectionOfBooksThatBelongToThisSeries()
        {
            var series = new Series();
            series.BooksInSeries = new List<Book>();

            series.BooksInSeries.Should().AllBeAssignableTo<Book>();
        }
    }
}
