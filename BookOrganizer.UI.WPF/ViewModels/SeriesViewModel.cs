﻿using BookOrganizer.DA;
using BookOrganizer.Domain;
using Prism.Events;
using System.Linq;
using System.Threading.Tasks;

namespace BookOrganizer.UI.WPF.ViewModels
{
    public class SeriesViewModel : BaseViewModel<Series>
    {
        private readonly ISeriesLookupDataService seriesLookupDataService;

        public SeriesViewModel(IEventAggregator eventAggregator, ISeriesLookupDataService seriesLookupDataService)
            : base(eventAggregator)
        {
            this.seriesLookupDataService = seriesLookupDataService;

            Init();
        }

        public Task Init()
        {
            return InitializeRepositoryAsync();
        }

        public async override Task InitializeRepositoryAsync()
        {
            Items = await seriesLookupDataService.GetSeriesLookupAsync(nameof(SeriesDetailViewModel));

            EntityCollection = Items.OrderBy(p => p.DisplayMember).ToList();
        }
    }
}
