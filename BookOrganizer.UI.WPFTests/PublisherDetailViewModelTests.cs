using BookOrganizer.Domain;
using BookOrganizer.UI.WPF.Enums;
using BookOrganizer.Data.Repositories;
using BookOrganizer.UI.WPF.Services;
using BookOrganizer.UI.WPF.ViewModels;
using BookOrganizer.UI.WPFTests.Extensions;
using FluentAssertions;
using Moq;
using Prism.Events;
using System;
using System.Windows.Media;
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

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void WhenPublisherNameIsSetNullOrEmpty_ThrowsArgumentOutOfRangeException(string name)
        {
            Action action = () => viewModel.Name = name;

            action.Should().Throw<ArgumentOutOfRangeException>();
        }

        [Fact]
        public void TryingToSetPublisherNameLongerThan64Characters_ThrowsArgumentOutOfRangeException()
        {
            Action action = ()
                => viewModel.Name = "Spicy jalapeno bacon ipsum dolor amet prosciutto swine andouille hamburger tri-tip ground round pork";

            action.Should().Throw<ArgumentOutOfRangeException>();
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

        [Fact]
        public void NewPublishersId_ShouldHaveDefaultValue()
        {
            viewModel.SelectedItem.Should().BeOfType<Publisher>();
            viewModel.SelectedItem.Id.Should().Equals(default(Guid));
        }

        [Fact]
        public async void OpeningAPublisherDetailViewWithANewPublisher_ShouldByDefaultOpenAsEditable()
        {
            await viewModel.LoadAsync(default(Guid));
            viewModel.UserMode.Item1.Should().BeFalse();
            viewModel.UserMode.Item2.Should().Equals(DetailViewState.EditMode);
            viewModel.UserMode.Item3.Should().Equals(Brushes.LightGreen);
            viewModel.UserMode.Item4.Should().BeTrue();
        }

        [Fact]
        public async void NewPublisherLogoPath_ShouldBeSetToPlaceholderImage()
        {
            await viewModel.LoadAsync(default);
            viewModel.SelectedItem.LogoPath.Should().EndWith("placeholder.png");
        }
    }
}
