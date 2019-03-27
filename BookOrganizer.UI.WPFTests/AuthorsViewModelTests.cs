using BookOrganizer.UI.WPF.Lookups;
using BookOrganizer.UI.WPF.ViewModels;
using FluentAssertions;
using Moq;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace BookOrganizer.UI.WPFTests
{
    public class AuthorsViewModelTests
    {
        private Mock<IEventAggregator> eventAggregatorMock;
        private Mock<IAuthorLookupDataService> authorLookupServiceMock;
        private AuthorsViewModel viewModel;

        public AuthorsViewModelTests()
        {
            eventAggregatorMock = new Mock<IEventAggregator>();
            authorLookupServiceMock = new Mock<IAuthorLookupDataService>();

            authorLookupServiceMock.Setup(dp => dp.GetAuthorLookupAsync())
                .ReturnsAsync(new List<LookupItem>
                {
                    new LookupItem { Id = Guid.NewGuid(), DisplayMember = "King, Stephen" },
                    new LookupItem { Id = Guid.NewGuid(), DisplayMember = "Rothfuss, Patrick" }
                });

            viewModel = new AuthorsViewModel(eventAggregatorMock.Object, authorLookupServiceMock.Object);
        }

        [Fact]
        public async Task ShouldLoad_AuthorLookupItems_WhenInitializeRepositoryAsyncIsCalled()
        {
            await viewModel.InitializeRepositoryAsync();

            viewModel.EntityCollection.Count.Should().Equals(2);

            var book = viewModel.EntityCollection.SingleOrDefault(f => f.DisplayMember == "King, Stephen");

            book.Should().NotBeNull();
            book.DisplayMember.Should().BeEquivalentTo("King, Stephen");
        }
    }
}
