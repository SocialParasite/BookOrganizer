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
    public class BookDetailViewModelTests
    {
        private Mock<IEventAggregator> eventAggregatorMock;
        private Mock<IMetroDialogService> metroDialogServiceMock;
        private Mock<IRepository<Book>> booksRepoMock;
        private Mock<ILanguageLookupDataService> languageLookupDataServiceMock;
        private Mock<IPublisherLookupDataService> publisherLookupDataServiceMock;
        private Mock<IAuthorLookupDataService> authorLookupDataServiceMock;
        private Mock<IFormatLookupDataService> formatLookupDataServiceMock;
        private BookDetailViewModel viewModel;

        public BookDetailViewModelTests()
        {
            eventAggregatorMock = new Mock<IEventAggregator>();
            metroDialogServiceMock = new Mock<IMetroDialogService>();
            booksRepoMock = new Mock<IRepository<Book>>();
            languageLookupDataServiceMock = new Mock<ILanguageLookupDataService>();
            publisherLookupDataServiceMock = new Mock<IPublisherLookupDataService>();
            authorLookupDataServiceMock = new Mock<IAuthorLookupDataService>();
            formatLookupDataServiceMock = new Mock<IFormatLookupDataService>();

            viewModel = new BookDetailViewModel(eventAggregatorMock.Object,
                metroDialogServiceMock.Object,
                booksRepoMock.Object,
                languageLookupDataServiceMock.Object,
                publisherLookupDataServiceMock.Object,
                authorLookupDataServiceMock.Object,
                formatLookupDataServiceMock.Object);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void WhenBookTitleIsSetNullOrEmpty_ThrowsArgumentOutOfRangeException(string name)
        {
            Action action = () => viewModel.Title = name;

            action.Should().Throw<ArgumentOutOfRangeException>();
        }

        [Fact]
        public void WhenTryingToSetBookTitleLongerThan256Characters_ThrowsArgumentOutOfRangeException()
        {
            Action action = ()
                => viewModel.Title = "Spicy jalapeno bacon ipsum dolor amet prosciutto swine andouille hamburger tri-tip ground round pork " +
                "belly. Capicola chuck andouille, short ribs turducken salami short loin filet mignon biltong pork belly fatback. " +
                "Drumstick jowl pork chop, short ribs prosciutto picanha pork landjaeger pork loin.";

            action.Should().Throw<ArgumentOutOfRangeException>();
        }

        [Theory]
        [InlineData(0)]
        [InlineData(2501)]
        public void TryingToSetReleaseYearLessThan1OrMoreThan2500_ThrowArgumentOutException(int year)
        {
            Action action = () => viewModel.ReleaseYear = year;

            action.Should().Throw<ArgumentOutOfRangeException>();
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        [InlineData(11_546)]
        [InlineData(10_001)]
        public void TryingToSetBookPageCountLessThanOneOrOver10000_ThrowsArgumentOutOfRangeException(int pageCount)
        {
            Action action = () => viewModel.PageCount = pageCount;

            action.Should().Throw<ArgumentOutOfRangeException>();
        }

        [Theory]
        [InlineData("0553103547")]
        [InlineData("000224585X")]
        [InlineData("0553106635")]
        [InlineData("0002247437")]
        [InlineData("9780553801477")]
        [InlineData("9781566199094")]
        [InlineData("9781402894626")]
        [InlineData("")]
        public void BookIsbnAcceptsStandardDefinedISBN10Or13ValueOrEmptyString(string isbn)
        {
            viewModel.ISBN = isbn;

            viewModel.ISBN.Should().BeOfType(typeof(string)).And.Equals(isbn);
        }

        [Theory]
        [InlineData("9780553801477Y")]
        [InlineData("000224585Z")]
        [InlineData("ABCDEFGHI")]
        [InlineData("978055380147XX")]
        [InlineData("000224585ZRR")]
        [InlineData("ABCDEFGHIJKLM")]
        public void TryingToSetBookIsbnNonStandardOrNonEmptyValue_ThrowArgumentOutOfRangeException(string isbn)
        {
            Action action = () => viewModel.ISBN = isbn;

            action.Should().Throw<ArgumentOutOfRangeException>();
        }

        [Theory]
        [InlineData(-1)]
        public void TryingToSetBookWordCountNegative_ThrowsArgumentOutOfRangeException(int wordCount)
        {
            Action action = () => viewModel.WordCount = wordCount;

            action.Should().Throw<ArgumentOutOfRangeException>();
        }


        [Theory]
        [InlineData("SelectedReleaseYear")]
        [InlineData("SelectedBookId")]
        [InlineData("SelectedPublisherId")]
        [InlineData("SelectedAuthorId")]
        [InlineData("SelectedSeriesId")]
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

        [Fact]
        public void NewBooksId_ShouldHaveDefaultValue()
        {
            viewModel.SelectedItem.Should().BeOfType<Book>();
            viewModel.SelectedItem.Id.Should().Equals(default(Guid));
        }

        [Fact]
        public async void OpeningBookDetailViewWithANewBook_ShouldByDefaultOpenAsEditable()
        {
            await viewModel.LoadAsync(default);
            viewModel.UserMode.Item1.Should().BeFalse();
            viewModel.UserMode.Item2.Should().Equals(DetailViewState.EditMode);
            viewModel.UserMode.Item3.Should().Equals(Brushes.LightGreen);
            viewModel.UserMode.Item4.Should().BeTrue();
        }

        [Fact]
        public async void NewBookCoverPath_ShouldBeSetToPlaceholderImage()
        {
            await viewModel.LoadAsync(default);
            viewModel.SelectedItem.BookCoverPicturePath.Should().EndWith("placeholder.png");
        }
    }
}
