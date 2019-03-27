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
    public class PublisherDetailViewModelTests
    {
        private Mock<IEventAggregator> eventAggregatorMock;
        private Mock<IMetroDialogService> metroDialogServiceMock;
        private Mock<IRepository<Publisher>> publishersRepoMock;
        private PublisherDetailViewModel viewModel;

        public PublisherDetailViewModelTests()
        {
            eventAggregatorMock = new Mock<IEventAggregator>();
            metroDialogServiceMock = new Mock<IMetroDialogService>();
            publishersRepoMock = new Mock<IRepository<Publisher>>();

            viewModel = new PublisherDetailViewModel(eventAggregatorMock.Object,
                metroDialogServiceMock.Object,
                publishersRepoMock.Object);
        }

        [Fact]
        public void Name_ShouldRaise_PropertyChangedEvent()
        {
            var raised = viewModel.IsPropertyChangedRaised(() =>
            {
                viewModel.Name = "Brand new title!";
            }, nameof(viewModel.Name));

            raised.Should().BeTrue();
        }
    }
}
