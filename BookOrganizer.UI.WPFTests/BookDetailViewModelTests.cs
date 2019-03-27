using BookOrganizer.Domain;
using BookOrganizer.UI.WPF.Lookups;
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
    public class BookDetailViewModelTests
    {
        private Mock<IEventAggregator> eventAggregatorMock;
        private Mock<IMetroDialogService> metroDialogServiceMock;
        private Mock<IRepository<Book>> booksRepoMock;
        private Mock<ILanguageLookupDataService> languageLookupDataServiceMock;
        private Mock<IPublisherLookupDataService> publisherLookupDataServiceMock;
        private Mock<IAuthorLookupDataService> authorLookupDataServiceMock;
        private BookDetailViewModel viewModel;

        public BookDetailViewModelTests()
        {
            eventAggregatorMock = new Mock<IEventAggregator>();
            metroDialogServiceMock = new Mock<IMetroDialogService>();
            booksRepoMock = new Mock<IRepository<Book>>();
            languageLookupDataServiceMock = new Mock<ILanguageLookupDataService>();
            publisherLookupDataServiceMock = new Mock<IPublisherLookupDataService>();
            authorLookupDataServiceMock = new Mock<IAuthorLookupDataService>();

            viewModel = new BookDetailViewModel(eventAggregatorMock.Object,
                metroDialogServiceMock.Object,
                booksRepoMock.Object,
                languageLookupDataServiceMock.Object,
                publisherLookupDataServiceMock.Object,
                authorLookupDataServiceMock.Object);
        }

        [Theory]
        [InlineData("SelectedReleaseYear")]
        [InlineData("SelectedBookId")]
        [InlineData("HighlightBrush")]
        [InlineData("SelectedLanguage")]
        [InlineData("SelectedPublisher")]
        [InlineData("SelectedAuthor")]
        [InlineData("NewReadDate")]
        public void ShouldRaise_PropertyChangedEvent(string propertyName)
        {
            var propertyInfo = viewModel.GetType().GetProperty(propertyName);
            var propertyType = viewModel.GetType().GetProperty(propertyName).PropertyType;
            var objectType = propertyType.Assembly.GetType(propertyType.FullName);
            var newObject = Activator.CreateInstance(objectType);

            var raised = viewModel.IsPropertyChangedRaised(() =>
            {
                propertyInfo.SetValue(viewModel, newObject);
            }, propertyInfo.Name);

            raised.Should().BeTrue();
        }

        [Fact]
        public void Title_ShouldRaise_PropertyChangedEvent()
        {
            var raised = viewModel.IsPropertyChangedRaised(() =>
            {
               viewModel.Title = "Brand new title!";
            }, nameof(viewModel.Title));

            raised.Should().BeTrue();
        }
    }
}
