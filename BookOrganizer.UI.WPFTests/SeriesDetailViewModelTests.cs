using BookOrganizer.Domain;
using BookOrganizer.UI.WPF.Enums;
using BookOrganizer.Data.Lookups;
using BookOrganizer.DA;
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

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void WhenSeriesNameIsSetNullOrEmpty_ThrowsArgumentOutOfRangeException(string name)
        {
            Action action = () => viewModel.Name = name;

            action.Should().Throw<ArgumentOutOfRangeException>();
        }

        [Fact]
        public void WhenTryingToSetSeriesNameLongerThan256Characters_ThrowsArgumentOutOfRangeException()
        {
            Action action = ()
                => viewModel.Name = "Spicy jalapeno bacon ipsum dolor amet prosciutto swine andouille hamburger tri-tip ground round pork " +
                "belly. Capicola chuck andouille, short ribs turducken salami short loin filet mignon biltong pork belly fatback. " +
                "Drumstick jowl pork chop, short ribs prosciutto picanha pork landjaeger pork loin.";

            action.Should().Throw<ArgumentOutOfRangeException>();
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

        [Fact]
        public void NewSeriesId_ShouldHaveDefaultValue()
        {
            viewModel.SelectedItem.Should().BeOfType<Series>();
            viewModel.SelectedItem.Id.Should().Equals(default(Guid));
        }

        [Fact]
        public void NewSeriesAllBooks_ShouldBeEmpty()
        {
            viewModel.AllBooks.Should().BeEmpty();
        }

        [Fact]
        public void NewSeriesBooks_ShouldBeEmpty()
        {
            viewModel.Books.Should().BeEmpty();
        }

        [Fact]
        public async void OpeningASeriesDetailViewWithANewSeries_ShouldByDefaultOpenAsEditable()
        {
            await viewModel.LoadAsync(default(Guid));
            viewModel.UserMode.Item1.Should().BeFalse();
            viewModel.UserMode.Item2.Should().Equals(DetailViewState.EditMode);
            viewModel.UserMode.Item3.Should().Equals(Brushes.LightGreen);
            viewModel.UserMode.Item4.Should().BeTrue();
        }

        [Fact]
        public async void NewSeriesPicturePath_ShouldBeSetToPlaceholderImage()
        {
            await viewModel.LoadAsync(default);
            viewModel.SelectedItem.PicturePath.Should().EndWith("placeholder.png");
        }

    }
}
