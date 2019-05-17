using BookOrganizer.Domain;
using BookOrganizer.UI.WPF.Lookups;
using Prism.Events;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BookOrganizer.UI.WPF.ViewModels
{
    public class PublishersViewModel : BaseViewModel<Publisher>, IPublishersViewModel
    {
        private readonly IPublisherLookupDataService publisherLookupDataService;

        public PublishersViewModel(IEventAggregator eventAggregator,
                              IPublisherLookupDataService publisherLookupDataService)
            : base(eventAggregator)
        {
            this.publisherLookupDataService = publisherLookupDataService
                ?? throw new ArgumentNullException(nameof(publisherLookupDataService));

            InitializeRepositoryAsync();
        }

        public override async Task InitializeRepositoryAsync()
        {
            Items = await publisherLookupDataService.GetPublisherLookupAsync();

            EntityCollection = Items.OrderBy(p => p.DisplayMember).ToList();
        }
    }
}
