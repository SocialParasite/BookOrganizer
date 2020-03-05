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
    public class FormatDetailViewModelTests
    {
        private FormatDetailViewModel viewModel;

        public FormatDetailViewModelTests()
        {
            var eventAggregatorMock = new Mock<IEventAggregator>();
            var loggerMock = new Mock<ILogger>();
            var formatLookupServiceMock = new Mock<IFormatLookupDataService>();
            var formatRepoMock = new Mock<IRepository<Format>>();
            var domainService = new FormatService(formatRepoMock.Object);

            viewModel = new FormatDetailViewModel(eventAggregatorMock.Object, 
                loggerMock.Object, 
                domainService,
                formatLookupServiceMock.Object);
        }

        [Fact]
        public void Name_ShouldRaise_PropertyChangedEvent()
        {
            var raised = viewModel.SelectedItem.IsPropertyChangedRaised(() =>
            {
                viewModel.SelectedItem.Name = "Pdf";
            }, nameof(viewModel.SelectedItem.Name));

            raised.Should().BeTrue();
        }

        [Fact]
        public void NewFormatId_ShouldHaveDefaultValue()
        {
            viewModel.SelectedItem.Model.Should().BeOfType<Format>();
            viewModel.SelectedItem.Model.Id.Should().Equals(default(Guid));
        }

        [Fact]
        public void NewFormatsCollection_ShouldBeEmpty()
        {
            viewModel.Formats.Should().BeEmpty();
        }

        [Fact]
        public async void OpeningAFormatDetailViewWithANewFormat_ShouldByDefaultOpenAsEditable()
        {
            await viewModel.LoadAsync(default);
            viewModel.UserMode.Item1.Should().BeFalse();
            viewModel.UserMode.Item2.Should().Equals(DetailViewState.EditMode);
            viewModel.UserMode.Item3.Should().Equals(Brushes.LightGreen);
            viewModel.UserMode.Item4.Should().BeTrue();
        }
    }
}
