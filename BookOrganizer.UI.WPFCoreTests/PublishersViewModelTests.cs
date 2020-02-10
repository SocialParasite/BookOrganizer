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
    public class PublishersViewModelTests
    {
        private PublishersViewModel CreateViewModelWithPublishers()
        {
            var eventAggregatorMock = new Mock<IEventAggregator>();
            var publisherLookupServiceMock = new Mock<IPublisherLookupDataService>();
            var loggerMock = new Mock<ILogger>();

            publisherLookupServiceMock.Setup(dp => dp.GetPublisherLookupAsync(nameof(PublisherDetailViewModel)))
                .ReturnsAsync(new List<LookupItem>
                {
                    new LookupItem { Id = Guid.NewGuid(), DisplayMember = "Publisher 1" },
                    new LookupItem { Id = Guid.NewGuid(), DisplayMember = "The Second Publisher" }
                });

            return new PublishersViewModel(eventAggregatorMock.Object, publisherLookupServiceMock.Object, loggerMock.Object);
        }

        [Fact]
        public async Task Get_All_Publishers()
        {
            var viewModel = CreateViewModelWithPublishers();
            viewModel.EntityCollection.Count.Should().Equals(0);

            await viewModel.InitializeRepositoryAsync();

            viewModel.EntityCollection.Count.Should().Equals(2);
        }

        [Fact]
        public async Task Publisher_In_Collection()
        {
            var viewModel = CreateViewModelWithPublishers();
            
            await viewModel.InitializeRepositoryAsync();
            var publisher = viewModel.EntityCollection.SingleOrDefault(f => f.DisplayMember == "Publisher 1");

            publisher.Should().NotBeNull();
            publisher.DisplayMember.Should().BeEquivalentTo("Publisher 1");
        }
    }
}
