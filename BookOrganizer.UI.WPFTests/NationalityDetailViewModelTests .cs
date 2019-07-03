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
    public class NationalityDetailViewModelTests
    {
        private Mock<IEventAggregator> eventAggregatorMock;
        private Mock<IMetroDialogService> metroDialogServiceMock;
        private Mock<IRepository<Nationality>> nationalitiesRepoMock;
        private Mock<INationalityLookupDataService> nationalityLookupServiceMock;
        private NationalityDetailViewModel viewModel;

        public NationalityDetailViewModelTests()
        {
            eventAggregatorMock = new Mock<IEventAggregator>();
            metroDialogServiceMock = new Mock<IMetroDialogService>();
            nationalitiesRepoMock = new Mock<IRepository<Nationality>>();
            nationalityLookupServiceMock = new Mock<INationalityLookupDataService>();

            viewModel = new NationalityDetailViewModel(eventAggregatorMock.Object,
                metroDialogServiceMock.Object,
                nationalitiesRepoMock.Object,
                nationalityLookupServiceMock.Object);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void WhenNationalityNameIsSetNullOrEmpty_ThrowsArgumentOutOfRangeException(string name)
        {
            Action action = () => viewModel.Name = name;

            action.Should().Throw<ArgumentOutOfRangeException>();
        }

        [Fact]
        public void TryingToSetNationalityNameLongerThan32Characters_ThrowsArgumentOutOfRangeException()
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
                viewModel.Name = "American";
            }, nameof(viewModel.Name));

            raised.Should().BeTrue();
        }


        [Fact]
        public void NewNationalityId_ShouldHaveDefaultValue()
        {
            viewModel.SelectedItem.Should().BeOfType<Nationality>();
            viewModel.SelectedItem.Id.Should().Equals(default(Guid));
        }

        [Fact]
        public void NewNationalitiesNationsCollection_ShouldBeEmpty()
        {
            viewModel.Nations.Should().BeEmpty();
        }

        [Fact]
        public async void OpeningANationalityDetailViewWithANewNationality_ShouldByDefaultOpenAsEditable()
        {
            await viewModel.LoadAsync(default);
            viewModel.UserMode.Item1.Should().BeFalse();
            viewModel.UserMode.Item2.Should().Equals(DetailViewState.EditMode);
            viewModel.UserMode.Item3.Should().Equals(Brushes.LightGreen);
            viewModel.UserMode.Item4.Should().BeTrue();
        }

    }
}
