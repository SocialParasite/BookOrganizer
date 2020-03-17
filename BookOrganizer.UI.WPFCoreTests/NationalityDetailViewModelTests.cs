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
    public class NationalityDetailViewModelTests
    {
        private NationalityDetailViewModel viewModel;

        public NationalityDetailViewModelTests()
        {
            var eventAggregatorMock = new Mock<IEventAggregator>();
            var loggerMock = new Mock<ILogger>();
            var nationalityLookupServiceMock = new Mock<INationalityLookupDataService>();
            var nationalityRepoMock = new Mock<IRepository<Nationality>>();
            var domainService = new NationalityService(nationalityRepoMock.Object);
            var dialogService = new DialogService();

            viewModel = new NationalityDetailViewModel(eventAggregatorMock.Object,
                loggerMock.Object,
                domainService,
                nationalityLookupServiceMock.Object,
                dialogService);
        }

        [Fact]
        public void Name_ShouldRaise_PropertyChangedEvent()
        {
            var raised = viewModel.SelectedItem.IsPropertyChangedRaised(() =>
            {
                viewModel.SelectedItem.Name = "Canadian";
            }, nameof(viewModel.SelectedItem.Name));

            raised.Should().BeTrue();
        }

        [Fact]
        public void New_Nationality_Has_Default_Id()
        {
            viewModel.SelectedItem.Model.Should().BeOfType<Nationality>();
            viewModel.SelectedItem.Model.Id.Should().Equals(default(Guid));
        }

        [Fact]
        public void New_Nations_Nations_Collection_Is_Empty()
        {
            viewModel.Nations.Should().BeEmpty();
        }

        [Fact]
        public async void New_Nationality_In_Editable_State_By_Default()
        {
            await viewModel.LoadAsync(default);
            viewModel.UserMode.Item1.Should().BeFalse();
            viewModel.UserMode.Item2.Should().Equals(DetailViewState.EditMode);
            viewModel.UserMode.Item3.Should().Equals(Brushes.LightGreen);
            viewModel.UserMode.Item4.Should().BeTrue();
        }
    }
}
