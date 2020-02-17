using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookOrganizer.Domain;
using BookOrganizer.UI.WPF.ViewModels;
using FluentAssertions;
using Moq;
using Prism.Events;
using Xunit;

namespace BookOrganizer.UI.WPFTests
{
    public class BooksViewModelTests
    {
        private Mock<IEventAggregator> eventAggregatorMock;
        private Mock<IBookLookupDataService> bookLookupServiceMock;
        private BooksViewModel viewModel;

        public BooksViewModelTests()
        {
            eventAggregatorMock = new Mock<IEventAggregator>();
            bookLookupServiceMock = new Mock<IBookLookupDataService>();

            bookLookupServiceMock.Setup(dp => dp.GetBookLookupAsync(nameof(BookDetailViewModel)))
                .ReturnsAsync(new List<LookupItem>
                {
                    new LookupItem { Id = Guid.NewGuid(), DisplayMember = "The Book" },
                    new LookupItem { Id = Guid.NewGuid(), DisplayMember = "The Book: Sequel" }
                });

            viewModel = new BooksViewModel(eventAggregatorMock.Object, bookLookupServiceMock.Object);
        }

        [Fact]
        public async Task ShouldLoad_BookLookupItems_WhenInitializeRepositoryAsyncIsCalled()
        {
            await viewModel.InitializeRepositoryAsync();

            viewModel.EntityCollection.Count.Should().Equals(2);

            var book = viewModel.EntityCollection.SingleOrDefault(f => f.DisplayMember == "The Book");

            book.Should().NotBeNull();
            book.DisplayMember.Should().BeEquivalentTo("The Book");
        }
    }
}
