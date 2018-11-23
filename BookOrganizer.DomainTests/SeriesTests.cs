using BookOrganizer.Domain;
using FluentAssertions;
using System;
using Xunit;

namespace BookOrganizer.DomainTests
{
    public class SeriesTests
    {
        [Theory]
        [InlineData(0)]
        [InlineData(-10)]
        public void TryingToAddNegativeOrZeroAmountOfBooksInTheSeries_ThrowsArgumentOutOfRangeException(int amount)
        {
            var series = new Series();

            Action action = () => series.NumberOfBooks = amount;
            action.Should().Throw<ArgumentOutOfRangeException>();
        }
    }
}
