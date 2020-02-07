using BookOrganizer.DA;
using BookOrganizer.Domain;
using Prism.Events;
using Serilog;
using System.Linq;
using System.Threading.Tasks;

namespace BookOrganizer.UI.WPFCore.ViewModels
{
    public class SeriesViewModel : BaseViewModel<Series>
    {
        private readonly ISeriesLookupDataService seriesLookupDataService;

        public SeriesViewModel(IEventAggregator eventAggregator, 
                               ISeriesLookupDataService seriesLookupDataService,
                               ILogger logger)
            : base(eventAggregator, logger)
        {
            this.seriesLookupDataService = seriesLookupDataService;

            Init();
        }

        private Task Init() 
            => InitializeRepositoryAsync();

        public async override Task InitializeRepositoryAsync()
        {
            Items = await seriesLookupDataService.GetSeriesLookupAsync(nameof(SeriesDetailViewModel)).ConfigureAwait(false);

            EntityCollection = Items.OrderBy(p => p.DisplayMember).ToList();
        }
    }
}
