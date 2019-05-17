using BookOrganizer.Domain;
using BookOrganizer.UI.WPF.Events;
using BookOrganizer.UI.WPF.Lookups;
using Prism.Commands;
using Prism.Events;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

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

            //PublisherNameLabelMouseLeftButtonUpCommand =
            //    new DelegateCommand<Guid?>(OnPublisherNameLabelMouseLeftButtonUpExecute,
            //                               OnPublisherNameLabelMouseLeftButtonUpCanExecute);

            InitializeRepositoryAsync();
        }

        //public ICommand PublisherNameLabelMouseLeftButtonUpCommand { get; set; }

        //private bool OnPublisherNameLabelMouseLeftButtonUpCanExecute(Guid? id)
        //    => (id is null || id == Guid.Empty) ? false : true;

        //private void OnPublisherNameLabelMouseLeftButtonUpExecute(Guid? id)
        //{
        //    eventAggregator.GetEvent<OpenDetailViewEvent>()
        //               .Publish(new OpenDetailViewEventArgs
        //               {
        //                   Id = (Guid)id,
        //                   ViewModelName = nameof(PublisherDetailViewModel)
        //               });
        //}

        public override async Task InitializeRepositoryAsync()
        {
            Items = await publisherLookupDataService.GetPublisherLookupAsync();

            EntityCollection = Items.OrderBy(p => p.DisplayMember).ToList();
        }
    }
}
