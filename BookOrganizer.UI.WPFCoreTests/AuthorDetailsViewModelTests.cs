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
        private Mock<IEventAggregator> eventAggregatorMock;
        private Mock<ILogger> loggerMock;
        private Mock<IDomainService<Author>> authorsDomainServiceMock;
        private AuthorDetailViewModel viewModel;

        public AuthorDetailViewModelTests()
        {
            eventAggregatorMock = new Mock<IEventAggregator>();
            loggerMock = new Mock<ILogger>();
            authorsDomainServiceMock = new Mock<IDomainService<Author>>();

            viewModel = new AuthorDetailViewModel(eventAggregatorMock.Object,
                loggerMock.Object,
                authorsDomainServiceMock.Object);
        }

        [Fact]
        public void NewAuthorsId_ShouldHaveDefaultValue()
        {
            viewModel.SelectedItem.Should().BeOfType<AuthorWrapper>();
            viewModel.SelectedItem.Id.Should().Equals(default(Guid));
        }

        [Fact]
        public async void OpeningAuthorDetailViewWithNewAuthor_ShouldByDefaultOpenAsEditable()
        {

            await viewModel.LoadAsync(default);
            viewModel.UserMode.Item1.Should().BeFalse();
            viewModel.UserMode.Item2.Should().Equals(DetailViewState.EditMode);
            viewModel.UserMode.Item3.Should().Equals(Brushes.LightGreen);
            viewModel.UserMode.Item4.Should().BeTrue();
        }

        [Fact]
        public async void NewAuthorMugShotPath_ShouldBeSetToPlaceholderImage()
        {
            await viewModel.LoadAsync(default);
            viewModel.SelectedItem.MugShotPath.Should().EndWith("placeholder.png");
        }

        [Fact]
        public void NewAuthorNationalities_ShouldBeEmpty()
        {
            viewModel.Nationalities.Should().BeEmpty();
        }
    }
}
