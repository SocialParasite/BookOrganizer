using BookOrganizer.Domain;
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
        private SeriesDetailViewModel viewModel;

        public SeriesDetailViewModelTests()
        {
            eventAggregatorMock = new Mock<IEventAggregator>();
            metroDialogServiceMock = new Mock<IMetroDialogService>();
            seriesRepoMock = new Mock<IRepository<Series>>();

            viewModel = new SeriesDetailViewModel(eventAggregatorMock.Object,
                metroDialogServiceMock.Object,
                seriesRepoMock.Object);
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
