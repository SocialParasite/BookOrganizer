using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookOrganizer.Domain;
using BookOrganizer.UI.WPF.ViewModels;
using FluentAssertions;
using Moq;
using Prism.Events;
using Xunit;

namespace BookOrganizer.UI.WPFTests
{
    public class SeriesViewModelTests
    {
        private Mock<IEventAggregator> eventAggregatorMock;
        private Mock<ISeriesLookupDataService> seriesLookupServiceMock;
        private SeriesViewModel viewModel;

        public SeriesViewModelTests()
        {
            eventAggregatorMock = new Mock<IEventAggregator>();
            seriesLookupServiceMock = new Mock<ISeriesLookupDataService>();

            seriesLookupServiceMock.Setup(dp => dp.GetSeriesLookupAsync(nameof(SeriesDetailViewModel)))
                .ReturnsAsync(new List<LookupItem>
                {
                    new LookupItem { Id = Guid.NewGuid(), DisplayMember = "A Song of Ice and Fire" },
                    new LookupItem { Id = Guid.NewGuid(), DisplayMember = "The Powder Mage Trilogy" }
                });

            viewModel = new SeriesViewModel(eventAggregatorMock.Object, seriesLookupServiceMock.Object);
        }

        [Fact]
        public async Task ShouldLoad_SeriesLookupItems_WhenInitializeRepositoryAsyncIsCalled()
        {
            await viewModel.InitializeRepositoryAsync();

            viewModel.EntityCollection.Count.Should().Equals(2);

            var book = viewModel.EntityCollection.SingleOrDefault(f => f.DisplayMember == "A Song of Ice and Fire");

            book.Should().NotBeNull();
            book.DisplayMember.Should().BeEquivalentTo("A Song of Ice and Fire");
        }
    }
}
