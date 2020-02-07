using BookOrganizer.DA;
using BookOrganizer.UI.WPFCore.ViewModels;
using FluentAssertions;
using Moq;
using Prism.Events;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

            var book = viewModel.EntityCollection.SingleOrDefault(f => f.DisplayMember == "A Song of Ice and Fire");

            book.Should().NotBeNull();
            book.DisplayMember.Should().BeEquivalentTo("A Song of Ice and Fire");
        }
    }
}
