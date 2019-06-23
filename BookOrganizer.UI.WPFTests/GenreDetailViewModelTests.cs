using BookOrganizer.Domain;
using BookOrganizer.UI.WPF.Enums;
using BookOrganizer.UI.WPF.Lookups;
using BookOrganizer.UI.WPF.Repositories;
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
    public class GenreDetailViewModelTests
    {
        private Mock<IEventAggregator> eventAggregatorMock;
        private Mock<IMetroDialogService> metroDialogServiceMock;
        private Mock<IRepository<Genre>> genreRepoMock;
        private Mock<IGenreLookupDataService> genreLookupServiceMock;
        private GenreDetailViewModel viewModel;

        public GenreDetailViewModelTests()
        {
            eventAggregatorMock = new Mock<IEventAggregator>();
            metroDialogServiceMock = new Mock<IMetroDialogService>();
            genreRepoMock = new Mock<IRepository<Genre>>();
            genreLookupServiceMock = new Mock<IGenreLookupDataService>();

            viewModel = new GenreDetailViewModel(eventAggregatorMock.Object,
                metroDialogServiceMock.Object,
                genreRepoMock.Object,
                genreLookupServiceMock.Object);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void WhenNameIsSetNullOrEmpty_ThrowsArgumentOutOfRangeException(string name)
        {
            Action action = () => viewModel.Name = name;

            action.Should().Throw<ArgumentOutOfRangeException>();
        }

        [Fact]
        public void TryingToSetGenreNameLongerThan32Characters_ThrowsArgumentOutOfRangeException()
        {
            Action action = ()
                => viewModel.Name = "Spicy jalapeno bacon ipsum dolor amet.";

            action.Should().Throw<ArgumentOutOfRangeException>();
        }

        [Fact]
        public void Name_ShouldRaise_PropertyChangedEvent()
        {
            var raised = viewModel.IsPropertyChangedRaised(() =>
            {
                viewModel.Name = "sci-fi";
            }, nameof(viewModel.Name));

            raised.Should().BeTrue();
        }


        [Fact]
        public void NewGenreId_ShouldHaveDefaultValue()
        {
            viewModel.SelectedItem.Should().BeOfType<Genre>();
            viewModel.SelectedItem.Id.Should().Equals(default(Guid));
        }

        [Fact]
        public void NewGenresCollection_ShouldBeEmpty()
        {
            viewModel.Genres.Should().BeEmpty();
        }

        [Fact]
        public async void OpeningAGenreDetailViewWithANewGenre_ShouldByDefaultOpenAsEditable()
        {
            await viewModel.LoadAsync(default);
            viewModel.UserMode.Item1.Should().BeFalse();
            viewModel.UserMode.Item2.Should().Equals(DetailViewState.EditMode);
            viewModel.UserMode.Item3.Should().Equals(Brushes.LightGreen);
            viewModel.UserMode.Item4.Should().BeTrue();
        }

    }
}
