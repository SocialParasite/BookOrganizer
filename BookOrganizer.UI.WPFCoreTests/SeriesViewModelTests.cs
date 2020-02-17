using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookOrganizer.Domain;
using BookOrganizer.UI.WPFCore.ViewModels;
using FluentAssertions;
using Moq;
using Prism.Events;
using Serilog;
using Xunit;

namespace BookOrganizer.UI.WPFCoreTests
{
    public class SeriesViewModelTests
    {
        private SeriesViewModel CreateViewModelWithSeries()
        {
            var eventAggregatorMock = new Mock<IEventAggregator>();
            var seriesLookupServiceMock = new Mock<ISeriesLookupDataService>();
            var loggerMock = new Mock<ILogger>();

            seriesLookupServiceMock.Setup(dp => dp.GetSeriesLookupAsync(nameof(SeriesDetailViewModel)))
                .ReturnsAsync(new List<LookupItem>
                {
                    new LookupItem { Id = Guid.NewGuid(), DisplayMember = "A Song of Ice and Fire" },
                    new LookupItem { Id = Guid.NewGuid(), DisplayMember = "The Powder Mage Trilogy" }
                });

            return new SeriesViewModel(eventAggregatorMock.Object, seriesLookupServiceMock.Object, loggerMock.Object);
        }

        [Fact]
        public async Task Get_All_Series()
        {
            var viewModel = CreateViewModelWithSeries();
            viewModel.EntityCollection.Count.Should().Equals(0);
            await viewModel.InitializeRepositoryAsync();

            viewModel.EntityCollection.Count.Should().Equals(2); 
        }

        [Fact]
        public async Task Series_In_Collection()
        {
            var viewModel = CreateViewModelWithSeries();
            await viewModel.InitializeRepositoryAsync();

            var series = viewModel.EntityCollection.SingleOrDefault(f => f.DisplayMember == "A Song of Ice and Fire");

            series.Should().NotBeNull();
            series.DisplayMember.Should().BeEquivalentTo("A Song of Ice and Fire");
        }
    }
}
