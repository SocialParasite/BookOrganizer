using BookOrganizer.Domain;
using BookOrganizer.Domain.Services;
using FluentAssertions;
using Xunit;
using Moq;

namespace BookOrganizer.DomainTests
{
    public class BookServiceTests
    {
        private BookService bookService;

        public BookServiceTests()
        {
            var repositoryMock = new Mock<IRepository<Book>>();
            var formatRepositoryMock = new Mock<IFormatRepository>();
            var genreRepositoryMock = new Mock<IGenreRepository>();
            var languageLookupDataServiceMock = new Mock<ILanguageLookupDataService>();
            var publisherLookupDataServiceMock = new Mock<IPublisherLookupDataService>();
            var authorLookupDataServiceMock = new Mock<IAuthorLookupDataService>();
            var formatLookupDataServiceMock = new Mock<IFormatLookupDataService>();
            var genreLookupDataServiceMock = new Mock<IGenreLookupDataService>();

            bookService = new BookService(repositoryMock.Object, 
                                          formatRepositoryMock.Object, 
                                          genreRepositoryMock.Object,
                                          languageLookupDataServiceMock.Object, 
                                          publisherLookupDataServiceMock.Object, 
                                          authorLookupDataServiceMock.Object,
                                          formatLookupDataServiceMock.Object, 
                                          genreLookupDataServiceMock.Object);
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
            var result = bookService.ValidateIsbn(isbn);
            result.Should().BeTrue();

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
            var result = bookService.ValidateIsbn(isbn);
            result.Should().BeFalse();
        }
    }
}
