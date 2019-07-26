using BookOrganizer.Domain;
using BookOrganizer.UI.WPF.Enums;
using BookOrganizer.Data.Lookups;
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
    public class FormatDetailViewModelTests
    {
        private Mock<IEventAggregator> eventAggregatorMock;
        private Mock<IMetroDialogService> metroDialogServiceMock;
        private Mock<IRepository<Format>> formatRepoMock;
        private Mock<IFormatLookupDataService> formatLookupServiceMock;
        private FormatDetailViewModel viewModel;

        public FormatDetailViewModelTests()
        {
            eventAggregatorMock = new Mock<IEventAggregator>();
            metroDialogServiceMock = new Mock<IMetroDialogService>();
            formatRepoMock = new Mock<IRepository<Format>>();
            formatLookupServiceMock = new Mock<IFormatLookupDataService>();

            viewModel = new FormatDetailViewModel(eventAggregatorMock.Object,
                metroDialogServiceMock.Object,
                formatRepoMock.Object,
                formatLookupServiceMock.Object);
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
        public void TryingToSetLanguageNameLongerThan32Characters_ThrowsArgumentOutOfRangeException()
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
                viewModel.Name = "Pdf";
            }, nameof(viewModel.Name));

            raised.Should().BeTrue();
        }


        [Fact]
        public void NewFormatId_ShouldHaveDefaultValue()
        {
            viewModel.SelectedItem.Should().BeOfType<Format>();
            viewModel.SelectedItem.Id.Should().Equals(default(Guid));
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
