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
    public class BookDetailViewModelTests
    {
        private BookDetailViewModel viewModel;

        public BookDetailViewModelTests()
        {
            var eventAggregatorMock = new Mock<IEventAggregator>();
            var booksRepoMock = new Mock<IRepository<Book>>();
            var languageLookupDataServiceMock = new Mock<ILanguageLookupDataService>();
            var publisherLookupDataServiceMock = new Mock<IPublisherLookupDataService>();
            var authorLookupDataServiceMock = new Mock<IAuthorLookupDataService>();
            var formatLookupDataServiceMock = new Mock<IFormatLookupDataService>();
            var genreLookupDataServiceMock = new Mock<IGenreLookupDataService>();
            var formatRepositoryMock = new Mock<IFormatRepository>();
            var genreRepositoryMock = new Mock<IGenreRepository>();

            var loggerMock = new Mock<ILogger>();
            var bookService = new BookService(booksRepoMock.Object, formatRepositoryMock.Object, genreRepositoryMock.Object, languageLookupDataServiceMock.Object, publisherLookupDataServiceMock.Object, authorLookupDataServiceMock.Object, formatLookupDataServiceMock.Object, genreLookupDataServiceMock.Object);
            
            viewModel = new BookDetailViewModel(eventAggregatorMock.Object, loggerMock.Object, bookService);
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
        public void Valid_ISBN(string isbn)
        {
            viewModel.SelectedItem.ISBN = isbn;

            viewModel.SelectedItem.ISBN.Should().BeOfType(typeof(string)).And.Equals(isbn);
        }

        [Theory]
        [InlineData("9780553801477Y")]
        [InlineData("000224585Z")]
        [InlineData("ABCDEFGHI")]
        [InlineData("978055380147XX")]
        [InlineData("000224585ZRR")]
        [InlineData("ABCDEFGHIJKLM")]
        public void Invalid_ISBN(string isbn)
        {
            var raised = viewModel.SelectedItem.IsPropertyChangedRaised(() =>
            {
                viewModel.SelectedItem.ISBN = isbn;
            }, nameof(viewModel.SelectedItem.ISBN));

            raised.Should().BeFalse();
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
        public void Raises_PropertyChangedEvent(string propertyName)
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
        public void Title_Changed_Raises_PropertyChangedEvent()
        {
            var raised = viewModel.SelectedItem.IsPropertyChangedRaised(() =>
            {
                viewModel.SelectedItem.Title = "Brand new title!";
            }, nameof(viewModel.SelectedItem.Title));

            raised.Should().BeTrue();
        }

        [Fact]
        public void New_Book_Has_Default_Guid_As_Id()
        {
            viewModel.SelectedItem.Should().BeOfType<BookWrapper>();
            viewModel.SelectedItem.Model.Id.Should().Equals(default(Guid));
        }

        [Fact]
        public async void New_Book_In_Editable_State_By_Default()
        {
            await viewModel.LoadAsync(default);
            viewModel.UserMode.Item1.Should().BeFalse();
            viewModel.UserMode.Item2.Should().Equals(DetailViewState.EditMode);
            viewModel.UserMode.Item3.Should().Equals(Brushes.LightGreen);
            viewModel.UserMode.Item4.Should().BeTrue();
        }

        [Fact]
        public async void New_Books_Cover_Image_Set_To_Placeholder_Image()
        {
            await viewModel.LoadAsync(default);
            viewModel.SelectedItem.BookCoverPicturePath.Should().EndWith("placeholder.png");
        }
    }
}
