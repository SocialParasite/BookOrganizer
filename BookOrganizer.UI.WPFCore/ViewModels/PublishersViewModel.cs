using BookOrganizer.DA;
using BookOrganizer.Domain;
using Prism.Events;
using Serilog;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BookOrganizer.UI.WPFCore.ViewModels
{
    public class PublishersViewModel : BaseViewModel<Publisher>
    {
        private readonly IPublisherLookupDataService publisherLookupDataService;

        public PublishersViewModel(IEventAggregator eventAggregator,
                              IPublisherLookupDataService publisherLookupDataService,
                              ILogger logger)
            : base(eventAggregator, logger)
        {
            this.publisherLookupDataService = publisherLookupDataService
                ?? throw new ArgumentNullException(nameof(publisherLookupDataService));

            Init();
        }

        private Task Init() 
            => InitializeRepositoryAsync();

        public override async Task InitializeRepositoryAsync()
        {
            Items = await publisherLookupDataService.GetPublisherLookupAsync(nameof(PublisherDetailViewModel));

            EntityCollection = Items.OrderBy(p => p.DisplayMember).ToList();
        }
    }
}
