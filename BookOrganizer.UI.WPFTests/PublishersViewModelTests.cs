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
    public class PublishersViewModelTests
    {
        private Mock<IEventAggregator> eventAggregatorMock;
        private Mock<IPublisherLookupDataService> publisherLookupServiceMock;
        private PublishersViewModel viewModel;

        public PublishersViewModelTests()
        {
            eventAggregatorMock = new Mock<IEventAggregator>();
            publisherLookupServiceMock = new Mock<IPublisherLookupDataService>();

            publisherLookupServiceMock.Setup(dp => dp.GetPublisherLookupAsync())
                .ReturnsAsync(new List<LookupItem>
                {
                    new LookupItem { Id = Guid.NewGuid(), DisplayMember = "Publisher 1" },
                    new LookupItem { Id = Guid.NewGuid(), DisplayMember = "The Second Publisher" }
                });

            viewModel = new PublishersViewModel(eventAggregatorMock.Object, publisherLookupServiceMock.Object);
        }

        [Fact]
        public async Task ShouldLoad_PublisherLookupItems_WhenInitializeRepositoryAsyncIsCalled()
        {
            await viewModel.InitializeRepositoryAsync();

            viewModel.EntityCollection.Count.Should().Equals(2);

            var book = viewModel.EntityCollection.SingleOrDefault(f => f.DisplayMember == "Publisher 1");

            book.Should().NotBeNull();
            book.DisplayMember.Should().BeEquivalentTo("Publisher 1");
        }
    }
}
