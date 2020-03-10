using System;
using System.Windows.Media;
using BookOrganizer.Domain;
using BookOrganizer.Domain.Services;
using BookOrganizer.UI.WPFCore;
using BookOrganizer.UI.WPFCore.ViewModels;
using FluentAssertions;
using Moq;
using Prism.Events;
using Serilog;
using Xunit;

namespace BookOrganizer.UI.WPFCoreTests
{
    public class LanguageDetailViewModelTests
    {
        private LanguageDetailViewModel viewModel;

        public LanguageDetailViewModelTests()
        {
            var eventAggregatorMock = new Mock<IEventAggregator>();
            var loggerMock = new Mock<ILogger>();
            var languageLookupServiceMock = new Mock<ILanguageLookupDataService>();
            var languageRepoMock = new Mock<IRepository<Language>>();
            var domainService = new LanguageService(languageRepoMock.Object);

            viewModel = new LanguageDetailViewModel(eventAggregatorMock.Object,
                loggerMock.Object,
                domainService,
                languageLookupServiceMock.Object);
        }

        [Fact]
        public void Name_ShouldRaise_PropertyChangedEvent()
        {
            var raised = viewModel.SelectedItem.IsPropertyChangedRaised(() =>
            {
                viewModel.SelectedItem.LanguageName = "Spanish";
            }, nameof(viewModel.SelectedItem.LanguageName));

            raised.Should().BeTrue();
        }

        [Fact]
        public void New_Language_Has_Default_Id()
        {
            viewModel.SelectedItem.Model.Should().BeOfType<Language>();
            viewModel.SelectedItem.Model.Id.Should().Equals(default(Guid));
        }

        [Fact]
        public void New_Languages_Languages_Collection_Is_Empty()
        {
            viewModel.Languages.Should().BeEmpty();
        }

        [Fact]
        public async void New_Language_In_Editable_State_By_Default()
        {
            await viewModel.LoadAsync(default);
            viewModel.UserMode.Item1.Should().BeFalse();
            viewModel.UserMode.Item2.Should().Equals(DetailViewState.EditMode);
            viewModel.UserMode.Item3.Should().Equals(Brushes.LightGreen);
            viewModel.UserMode.Item4.Should().BeTrue();
        }
    }
}
