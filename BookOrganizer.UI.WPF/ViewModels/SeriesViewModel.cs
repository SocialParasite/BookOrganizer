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
    public class SeriesViewModel : BaseViewModel<Series>, ISeriesViewModel
    {
        private readonly ISeriesLookupDataService seriesLookupDataService;

        public SeriesViewModel(IEventAggregator eventAggregator, ISeriesLookupDataService seriesLookupDataService)
            : base(eventAggregator)
        {
            this.seriesLookupDataService = seriesLookupDataService;

            SeriesNameLabelMouseLeftButtonUpCommand =
                new DelegateCommand<Guid?>(OnSeriesNameLabelMouseLeftButtonUpExecute,
                OnSeriesNameLabelMouseLeftButtonUpCanExecute);

            InitializeRepositoryAsync();
        }

        public ICommand SeriesNameLabelMouseLeftButtonUpCommand { get; }

        public async override Task InitializeRepositoryAsync()
        {
            Items = await seriesLookupDataService.GetSeriesLookupAsync();

            EntityCollection = Items.OrderBy(p => p.DisplayMember).ToList();
        }

        private bool OnSeriesNameLabelMouseLeftButtonUpCanExecute(Guid? id)
            => (id is null || id == Guid.Empty) ? false : true;

        private void OnSeriesNameLabelMouseLeftButtonUpExecute(Guid? id)
        {
            eventAggregator.GetEvent<OpenDetailViewEvent>()
                                   .Publish(new OpenDetailViewEventArgs
                                   {
                                       Id = (Guid)id,
                                       ViewModelName = nameof(SeriesDetailViewModel)
                                   });
        }
    }
}
