using BookOrganizer.Domain;
using BookOrganizer.UI.WPF.Lookups;
using Prism.Events;
using System.Linq;
using System.Threading.Tasks;

namespace BookOrganizer.UI.WPF.ViewModels
{
    public class SeriesViewModel : BaseViewModel<Series>, ISeriesViewModel
    {
        private readonly ISeriesLookupDataService seriesLookupDataService;

        public SeriesViewModel(IEventAggregator eventAggregator, ISeriesLookupDataService seriesLookupDataService)
            : base(eventAggregator)
        {
            this.seriesLookupDataService = seriesLookupDataService;

            InitializeRepositoryAsync();
        }

        public async override Task InitializeRepositoryAsync()
        {
            Items = await seriesLookupDataService.GetSeriesLookupAsync();

            EntityCollection = Items.OrderBy(p => p.DisplayMember).ToList();
        }
    }
}
