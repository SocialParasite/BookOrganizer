using BookOrganizer.Domain;
using BookOrganizer.UI.WPF.Events;
using BookOrganizer.UI.WPF.Lookups;
using Prism.Commands;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BookOrganizer.UI.WPF.ViewModels
{
    public class SeriesViewModel : BaseViewModel<Series>, ISeriesViewModel
    {
        private readonly ISeriesLookupDataService seriesLookupDataService;

        private OpenDetailViewEventArgs selectedSeries;

        public SeriesViewModel(IEventAggregator eventAggregator, ISeriesLookupDataService seriesLookupDataService)
            : base(eventAggregator)
        {
            this.seriesLookupDataService = seriesLookupDataService;

            SeriesNameLabelMouseLeftButtonUpCommand =
                new DelegateCommand<Guid?>(OnSeriesNameLabelMouseLeftButtonUpExecute,
                OnSeriesNameLabelMouseLeftButtonUpCanExecute);
            AddNewSeriesCommand = new DelegateCommand(OnAddNewSeriesExecute);

            InitializeRepositoryAsync();
        }

        public ICommand SeriesNameLabelMouseLeftButtonUpCommand { get; }
        public ICommand AddNewSeriesCommand { get; }

        public OpenDetailViewEventArgs SelectedSeries
        {
            get => selectedSeries;
            set
            {
                selectedSeries = value;
                OnPropertyChanged();

                if (selectedSeries != null)
                {
                    eventAggregator.GetEvent<OpenDetailViewEvent>()
                                   .Publish(selectedSeries);
                }
            }
        }

        public async override Task InitializeRepositoryAsync()
        {
            Items = await seriesLookupDataService.GetSeriesLookupAsync();

            EntityCollection = Items.OrderBy(p => p.DisplayMember).ToList();
        }

        private bool OnSeriesNameLabelMouseLeftButtonUpCanExecute(Guid? id)
            => (id is null || id == Guid.Empty) ? false : true;

        private void OnSeriesNameLabelMouseLeftButtonUpExecute(Guid? id)
            => SelectedSeries = new OpenDetailViewEventArgs { Id = (Guid)id, ViewModelName = nameof(SeriesDetailViewModel) };

        private void OnAddNewSeriesExecute()
            => SelectedSeries = new OpenDetailViewEventArgs { Id = new Guid(), ViewModelName = nameof(SeriesDetailViewModel) };
    }
}
