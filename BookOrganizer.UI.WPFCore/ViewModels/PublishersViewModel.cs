using BookOrganizer.DA;
using BookOrganizer.Domain;
using Prism.Events;
using Serilog;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

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

            ViewModelType = nameof(PublisherDetailViewModel);
        }

        private Task Init() 
            => InitializeRepositoryAsync();

        public override async Task InitializeRepositoryAsync()
        {
            try
            {
                Items = await publisherLookupDataService.GetPublisherLookupAsync(nameof(PublisherDetailViewModel));

                EntityCollection = Items.OrderBy(p => p.DisplayMember).ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                logger.Error("Message: {Message}\n\n Stack trace: {StackTrace}\n\n", ex.Message, ex.StackTrace);
            }
        }
    }
}
