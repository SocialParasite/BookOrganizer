using BookOrganizer.Domain;
using FluentAssertions;
using System;
using Xunit;

namespace BookOrganizer.DomainTests
{
    public class SeriesReadOrderTests
    {
        [Theory]
        [InlineData(1)]
        [InlineData(Int32.MaxValue)]
        public void InstalmentNumberShouldAcceptPositiveInteger(int instalment)
        {
            var seriesReadOrder = new SeriesReadOrder();
            seriesReadOrder.SeriesId = Guid.NewGuid();

            seriesReadOrder.Instalment = instalment;
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        public void TryingToSetInstalmentZeroOrBelow_ThrowsArgumentOutOfRangeException(int instalment)
        {
            var seriesReadOrder = new SeriesReadOrder();
            seriesReadOrder.SeriesId = Guid.NewGuid();

            Action action = () => seriesReadOrder.Instalment = instalment;

            action.Should().Throw<ArgumentOutOfRangeException>();
        }

        [Fact]
        public void TryingToSetInstalmentWhenSeriesIdIsNull_ThrowsArgumentException()
        {
            var seriesReadOrder = new SeriesReadOrder();

            Action action = () => seriesReadOrder.Instalment = 1;

            action.Should().Throw<ArgumentException>();
        }

    }
}
