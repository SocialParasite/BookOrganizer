using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Moq;
using Prism.Events;
using BookOrganizer.DA;
using BookOrganizer.UI.WPFCore.ViewModels;
using System.Threading.Tasks;
using FluentAssertions;
using System.Linq;
using Serilog;

namespace BookOrganizer.UI.WPFCoreTests
{
    public class AuthorsViewModelTests
    {
        private AuthorsViewModel CreateViewModelWithAuthors()
        {
            var eventAggregatorMock = new Mock<IEventAggregator>();
            var authorLookupServiceMock = new Mock<IAuthorLookupDataService>();
            var loggerMock = new Mock<ILogger>();

            authorLookupServiceMock.Setup(dp => dp.GetAuthorLookupAsync(nameof(AuthorDetailViewModel)))
                .ReturnsAsync(new List<LookupItem>
                {
                    new LookupItem { Id = Guid.NewGuid(), DisplayMember = "King, Stephen" },
                    new LookupItem { Id = Guid.NewGuid(), DisplayMember = "Rothfuss, Patrick" }
                });

            return new AuthorsViewModel(eventAggregatorMock.Object, authorLookupServiceMock.Object, loggerMock.Object);
        }

        [Fact]
        public async Task Get_All_Authors()
        {
            var viewModel = CreateViewModelWithAuthors();

            viewModel.EntityCollection.Count.Should().Equals(0);

            await viewModel.InitializeRepositoryAsync();
            viewModel.EntityCollection.Count.Should().Equals(2);
        }

        [Fact]
        public async Task Author_In_Collection()
        {
            var viewModel = CreateViewModelWithAuthors();
            await viewModel.InitializeRepositoryAsync();

            var book = viewModel.EntityCollection.SingleOrDefault(f => f.DisplayMember == "King, Stephen");

            book.Should().NotBeNull();
            book.DisplayMember.Should().BeEquivalentTo("King, Stephen");
        }
    }
}
