using Autofac.Features.Indexed;
using BookOrganizer.UI.WPF.Lookups;
using BookOrganizer.UI.WPF.ViewModels;
using BookOrganizer.UI.WPFTests.Extensions;
using FluentAssertions;
using Moq;
using Prism.Events;
using Xunit;

namespace BookOrganizer.UI.WPFTests
{
    public class MainViewModelTests
    {
        private Mock<IEventAggregator> eventAggregatorMock;
        private Mock<IBookLookupDataService> bookLookupServiceMock;
        private Mock<IIndex<string, IDetailViewModel>> detailViewModelCreatorMock;
        private Mock<IIndex<string, ISelectedViewModel>> viewModelCreatorMock;

        private MainViewModel viewModel;

        public MainViewModelTests()
        {
            eventAggregatorMock = new Mock<IEventAggregator>();
            detailViewModelCreatorMock = new Mock<IIndex<string, IDetailViewModel>>();
            viewModelCreatorMock = new Mock<IIndex<string, ISelectedViewModel>>();
            bookLookupServiceMock = new Mock<IBookLookupDataService>();

            viewModel = new MainViewModel(eventAggregatorMock.Object,
                detailViewModelCreatorMock.Object,
                viewModelCreatorMock.Object,
                bookLookupServiceMock.Object);
        }

        [Fact]
        public void IsViewVisible_ShouldRaise_PropertyChangedEvent()
        {
            viewModel.IsViewVisible = false;

            var raised = viewModel.IsPropertyChangedRaised(() =>
            {
                viewModel.IsViewVisible = true;
            }, nameof(viewModel.IsViewVisible));

            raised.Should().BeTrue();
        }

        [Fact]
        public void SelectedVM_ShouldRaise_PropertyChangedEvent()
        {
            var raised = viewModel.IsPropertyChangedRaised(() =>
            {
                viewModel.SelectedVM = default;
            }, nameof(viewModel.SelectedVM));

            raised.Should().BeTrue();
        }

        [Fact]
        public void SelectedDetailViewModel_ShouldRaise_PropertyChangedEvent()
        {
            var raised = viewModel.IsPropertyChangedRaised(() =>
            {
                viewModel.SelectedDetailViewModel = default;
            }, nameof(viewModel.SelectedDetailViewModel));

            raised.Should().BeTrue();
        }

        [Fact]
        public void NewMainView_DetailViewModels_ShouldBeEmpty()
        {
            viewModel.DetailViewModels.Should().BeEmpty();
        }

        [Fact]
        public void NewMainView_TEMP_DetailViewModels_ShouldBeEmpty()
        {
            viewModel.TEMP_DetailViewModels.Should().BeEmpty();
        }

        [Fact]
        public void NewSeriesIsMenuBarVisible_ShouldBeFalse()
        {
            viewModel.IsMenuBarVisible.Should().BeFalse();
        }
    }
}
