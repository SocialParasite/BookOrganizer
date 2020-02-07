using BookOrganizer.DA;
using BookOrganizer.UI.WPFCore.ViewModels;
using FluentAssertions;
using Moq;
using Prism.Events;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace BookOrganizer.UI.WPFCoreTests
{
    public class BooksViewModelTests
    {
        private BooksViewModel CreateViewModelWithBooks()
        {
            var eventAggregatorMock = new Mock<IEventAggregator>();
            var bookLookupServiceMock = new Mock<IBookLookupDataService>();
            var loggerMock = new Mock<ILogger>();

            bookLookupServiceMock.Setup(dp => dp.GetBookLookupAsync(nameof(BookDetailViewModel)))
                .ReturnsAsync(new List<LookupItem>
                {
                    new LookupItem { Id = Guid.NewGuid(), DisplayMember = "The Book" },
                    new LookupItem { Id = Guid.NewGuid(), DisplayMember = "The Book: Sequel" }
                });

            return new BooksViewModel(eventAggregatorMock.Object, bookLookupServiceMock.Object, loggerMock.Object);
        }

        [Fact]
        public async Task Get_All_Books()
        {
            var viewModel = CreateViewModelWithBooks();
            viewModel.EntityCollection.Count.Should().Equals(0);
            
            await viewModel.InitializeRepositoryAsync();
            viewModel.EntityCollection.Count.Should().Equals(2);

            var book = viewModel.EntityCollection.SingleOrDefault(f => f.DisplayMember == "The Book");

            book.Should().NotBeNull();
            book.DisplayMember.Should().BeEquivalentTo("The Book");
        }
    }
}
