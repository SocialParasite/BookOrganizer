using System;
using System.Windows.Media;
using BookOrganizer.Domain;
using BookOrganizer.Domain.Services;
using BookOrganizer.UI.WPFCore;
using BookOrganizer.UI.WPFCore.DialogServiceManager;
using BookOrganizer.UI.WPFCore.ViewModels;
using BookOrganizer.UI.WPFCore.Wrappers;
using FluentAssertions;
using Moq;
using Prism.Events;
using Serilog;
using Xunit;

namespace BookOrganizer.UI.WPFCoreTests
{
    public class GenreDetailViewModelTests
    {
        private GenreDetailViewModel viewModel;

        public GenreDetailViewModelTests()
        {
            var eventAggregatorMock = new Mock<IEventAggregator>();
            var loggerMock = new Mock<ILogger>();
            var genreLookupServiceMock = new Mock<IGenreLookupDataService>();
            var genreRepoMock = new Mock<IRepository<Genre>>();
            var domainService = new GenreService(genreRepoMock.Object);
            var dialogService = new DialogService();

            viewModel = new GenreDetailViewModel(eventAggregatorMock.Object,
                loggerMock.Object,
                domainService,
                genreLookupServiceMock.Object,
                dialogService);
        }

        [Fact]
        public void Name_ShouldRaise_PropertyChangedEvent()
        {
            var raised = viewModel.SelectedItem.IsPropertyChangedRaised(() =>
            {
                viewModel.SelectedItem.Name = "Horror";
            }, nameof(viewModel.SelectedItem.Name));

            raised.Should().BeTrue();
        }

        [Fact]
        public void New_Genre_Has_Default_Id()
        {
            viewModel.SelectedItem.Model.Should().BeOfType<Genre>();
            viewModel.SelectedItem.Model.Id.Should().Equals(default(Guid));
        }

        [Fact]
        public void New_Genres_Genres_Collection_Is_Empty()
        {
            viewModel.Genres.Should().BeEmpty();
        }

        [Fact]
        public async void New_Genre_In_Editable_State_By_Default()
        {
            await viewModel.LoadAsync(default);
            viewModel.UserMode.Item1.Should().BeFalse();
            viewModel.UserMode.Item2.Should().Equals(DetailViewState.EditMode);
            viewModel.UserMode.Item3.Should().Equals(Brushes.LightGreen);
            viewModel.UserMode.Item4.Should().BeTrue();
        }
    }
}
