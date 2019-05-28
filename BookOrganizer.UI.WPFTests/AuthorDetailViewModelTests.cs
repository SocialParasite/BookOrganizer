using BookOrganizer.Domain;
using BookOrganizer.UI.WPF.Repositories;
using BookOrganizer.UI.WPF.Services;
using BookOrganizer.UI.WPF.ViewModels;
using BookOrganizer.UI.WPFTests.Extensions;
using FluentAssertions;
using Moq;
using Prism.Events;
using System;
using Xunit;

namespace BookOrganizer.UI.WPFTests
{
    public class AuthorDetailViewModelTests
    {
        private Mock<IEventAggregator> eventAggregatorMock;
        private Mock<IMetroDialogService> metroDialogServiceMock;
        private Mock<IRepository<Author>> authorsRepoMock;
        private AuthorDetailViewModel viewModel;

        public AuthorDetailViewModelTests()
        {
            eventAggregatorMock = new Mock<IEventAggregator>();
            metroDialogServiceMock = new Mock<IMetroDialogService>();
            authorsRepoMock = new Mock<IRepository<Author>>();

            viewModel = new AuthorDetailViewModel(eventAggregatorMock.Object,
                metroDialogServiceMock.Object,
                authorsRepoMock.Object);
        }

        [Theory]
        [InlineData("")]
        [InlineData("Nicolaus Authoritus Maximus Nicolaus Authoritus Maximus")]
        public void GivenFirstNameLessThan1OrOver50Characters_ThrowsArgumentOutOfRangeException(string firstName)
        {
            Action action = () => viewModel.FirstName = firstName;

            action.Should().Throw<ArgumentOutOfRangeException>();
        }

        [Theory]
        [InlineData("")]
        [InlineData("Nicolaus Authoritus Maximus Nicolaus Authoritus Maximus")]
        public void GivenLastNameLessThan1OrOver50Characters_ThrowsArgumentOutOfRangeException(string lastName)
        {
            Action action = () => viewModel.LastName = lastName;

            action.Should().Throw<ArgumentOutOfRangeException>();
        }

        [Fact]
        public void FirstName_ShouldRaise_PropertyChangedEvent()
        {
            var raised = viewModel.IsPropertyChangedRaised(() =>
            {
                viewModel.FirstName = "Brandon";
            }, nameof(viewModel.FirstName));

            raised.Should().BeTrue();
        }

        [Fact]
        public void LastName_ShouldRaise_PropertyChangedEvent()
        {
            var raised = viewModel.IsPropertyChangedRaised(() =>
            {
                viewModel.LastName = "Sanderson";
            }, nameof(viewModel.LastName));

            raised.Should().BeTrue();
        }

        [Fact]
        public void NewAuthorsId_ShouldHaveDefaultValue()
        {
            viewModel.SelectedItem.Should().BeOfType<Author>();
            viewModel.SelectedItem.Id.Should().Equals(default(Guid));
        }
    }
}
