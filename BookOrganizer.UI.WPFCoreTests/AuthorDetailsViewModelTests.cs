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
    public class AuthorDetailViewModelTests
    {
        private AuthorDetailViewModel viewModel;

        public AuthorDetailViewModelTests()
        {
            var repMock = new Mock<IRepository<Author>>();
            var natMock = new Mock<INationalityLookupDataService>();
            var authorService = new AuthorService(repMock.Object, natMock.Object);
            var eventAggregatorMock = new Mock<IEventAggregator>();
            var loggerMock = new Mock<ILogger>();

            viewModel = new AuthorDetailViewModel(eventAggregatorMock.Object,
                loggerMock.Object,
                authorService);
        }

        [Fact]
        public void New_Author_Has_Default_Guid_As_Id()
        {
            viewModel.SelectedItem.Should().BeOfType<AuthorWrapper>();
            viewModel.SelectedItem.Id.Should().Equals(default(Guid));
        }

        [Fact]
        public async void New_Author_In_Editable_State_By_Default()
        {

            await viewModel.LoadAsync(default);
            viewModel.UserMode.Item1.Should().BeFalse();
            viewModel.UserMode.Item2.Should().Equals(DetailViewState.EditMode);
            viewModel.UserMode.Item3.Should().Equals(Brushes.LightGreen);
            viewModel.UserMode.Item4.Should().BeTrue();
        }

        [Fact]
        public async void New_Authors_Image_Set_To_Placeholder_Image()
        {
            await viewModel.LoadAsync(default);
            viewModel.SelectedItem.MugShotPath.Should().EndWith("placeholder.png");
        }

        [Fact]
        public void New_Authors_Nationality_Collection_Is_Empty()
        {
            viewModel.Nationalities.Should().BeEmpty();
        }
    }
}
