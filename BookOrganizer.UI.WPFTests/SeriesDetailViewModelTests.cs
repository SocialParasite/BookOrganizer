using BookOrganizer.Domain;
using BookOrganizer.UI.WPF.Lookups;
using BookOrganizer.UI.WPF.Repositories;
using BookOrganizer.UI.WPF.Services;
using BookOrganizer.UI.WPF.ViewModels;
using BookOrganizer.UI.WPFTests.Extensions;
using FluentAssertions;
using Moq;
using Prism.Events;
using Xunit;

namespace BookOrganizer.UI.WPFTests
{
    public class SeriesDetailViewModelTests
    {
        private Mock<IEventAggregator> eventAggregatorMock;
        private Mock<IMetroDialogService> metroDialogServiceMock;
        private Mock<IRepository<Series>> seriesRepoMock;
        private Mock<IBookLookupDataService> bookLookupDataServiceMock;
        private SeriesDetailViewModel viewModel;

        public SeriesDetailViewModelTests()
        {
            eventAggregatorMock = new Mock<IEventAggregator>();
            metroDialogServiceMock = new Mock<IMetroDialogService>();
            seriesRepoMock = new Mock<IRepository<Series>>();
            bookLookupDataServiceMock = new Mock<IBookLookupDataService>();

            viewModel = new SeriesDetailViewModel(
                eventAggregatorMock.Object,
                metroDialogServiceMock.Object,
                seriesRepoMock.Object,
                bookLookupDataServiceMock.Object);
        }

        [Fact]
        public void Name_ShouldRaise_PropertyChangedEvent()
        {
            var raised = viewModel.IsPropertyChangedRaised(() =>
            {
                viewModel.Name = "The Kingkiller Chronicle";
            }, nameof(viewModel.Name));

            raised.Should().BeTrue();
        }
    }
}
