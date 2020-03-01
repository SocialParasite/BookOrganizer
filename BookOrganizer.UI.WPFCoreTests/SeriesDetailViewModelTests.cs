using System;
using System.Windows.Media;
using BookOrganizer.Domain;
using BookOrganizer.Domain.Services;
using BookOrganizer.UI.WPFCore;
using BookOrganizer.UI.WPFCore.ViewModels;
using BookOrganizer.UI.WPFCore.Wrappers;
using FluentAssertions;
using Moq;
using Prism.Events;
using Serilog;
using Xunit;

namespace BookOrganizer.UI.WPFCoreTests
{

    public class SeriesDetailViewModelTests
    {
        private SeriesDetailViewModel viewModel;

        public SeriesDetailViewModelTests()
        {
            var seriesRepoMock = new Mock<IRepository<Series>>();
            var seriesService = new SeriesService(seriesRepoMock.Object);
            
            var eventAggregatorMock = new Mock<IEventAggregator>();
            var loggerMock = new Mock<ILogger>();
            var bookLookupDataServiceMock = new Mock<IBookLookupDataService>();

            viewModel = new SeriesDetailViewModel(eventAggregatorMock.Object, loggerMock.Object,
                    seriesService, bookLookupDataServiceMock.Object);
        }

        [Fact]
        public void New_Series_Has_Default_Id()
        {
            viewModel.SelectedItem.Should().BeOfType<SeriesWrapper>();
            viewModel.SelectedItem.Id.Should().Equals(default(Guid));
        }

        [Fact]
        public void New_Series_AllBooks_Collection_Is_Empty()
        {
            viewModel.AllBooks.Should().BeEmpty();
        }

        [Fact]
        public void New_Series_Book_Collection_Is_Empty()
        {
            viewModel.Books.Should().BeEmpty();
        }

        [Fact]
        public async void New_Series_In_Editable_State_By_Default()
        {
            await viewModel.LoadAsync(default(Guid));
            viewModel.UserMode.Item1.Should().BeFalse();
            viewModel.UserMode.Item2.Should().Equals(DetailViewState.EditMode);
            viewModel.UserMode.Item3.Should().Equals(Brushes.LightGreen);
            viewModel.UserMode.Item4.Should().BeTrue();
        }

        [Fact]
        public async void New_Series_Image_Is_Set_To_Placeholder_Image()
        {
            await viewModel.LoadAsync(default);
            viewModel.SelectedItem.PicturePath.Should().EndWith("placeholder.png");
        }
    }
}
