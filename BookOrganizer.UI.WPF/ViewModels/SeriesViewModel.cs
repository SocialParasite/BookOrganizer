﻿using BookOrganizer.Domain;
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
        private readonly IEventAggregator eventAggregator;
        private readonly ISeriesLookupDataService seriesLookupDataService;

        public SeriesViewModel(IEventAggregator eventAggregator, ISeriesLookupDataService seriesLookupDataService)
        {
            // TODO: Move to base
            this.eventAggregator = eventAggregator;
            this.seriesLookupDataService = seriesLookupDataService;

            SeriesNameLabelMouseLeftButtonUpCommand =
                new DelegateCommand<Guid?>(OnSeriesNameLabelMouseLeftButtonUpExecute, OnSeriesNameLabelMouseLeftButtonUpCanExecute);
            AddNewSeriesCommand = new DelegateCommand(OnAddNewSeriesExecute);

            InitializeRepositoryAsync();
        }

        private bool OnSeriesNameLabelMouseLeftButtonUpCanExecute(Guid? id)
            => (id is null || id == Guid.Empty) ? false : true;

        private void OnSeriesNameLabelMouseLeftButtonUpExecute(Guid? id)
        {
            throw new NotImplementedException();
            //=> SelectedSeries = new OpenDetailViewEventArgs { Id = (Guid)id, ViewModelName = nameof(SeriesDetailViewModel) };
        }

        private void OnAddNewSeriesExecute()
        {
            throw new NotImplementedException();
        }

        public ICommand SeriesNameLabelMouseLeftButtonUpCommand { get; }
        public ICommand AddNewSeriesCommand { get; }

        public async override Task InitializeRepositoryAsync()
        {
            Items = await seriesLookupDataService.GetSeriesLookupAsync();

            EntityCollection = Items.OrderBy(p => p.DisplayMember).ToList();
        }
    }
}
