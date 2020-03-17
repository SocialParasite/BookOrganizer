using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using BookOrganizer.Domain;
using BookOrganizer.UI.WPFCore.DialogServiceManager;
using Prism.Events;
using Serilog;

namespace BookOrganizer.UI.WPFCore.ViewModels
{
    public class SeriesViewModel : BaseViewModel<Series>
    {
        private readonly ISeriesLookupDataService seriesLookupDataService;

        public SeriesViewModel(IEventAggregator eventAggregator, 
                               ISeriesLookupDataService seriesLookupDataService,
                               ILogger logger,
                               IDialogService dialogService)
            : base(eventAggregator, logger, dialogService)
        {
            this.seriesLookupDataService = seriesLookupDataService;

            Init();

            ViewModelType = nameof(SeriesDetailViewModel);
        }

        private Task Init() 
            => InitializeRepositoryAsync();

        public async override Task InitializeRepositoryAsync()
        {
            try
            {
                Items = await seriesLookupDataService
                                .GetSeriesLookupAsync(nameof(SeriesDetailViewModel));

                EntityCollection = Items.OrderBy(p => p.DisplayMember).ToList();
            }
            catch (Exception ex)
            {
                var dialog = new NotificationViewModel("Exception", ex.Message);
                var result = dialogService.OpenDialog(dialog);

                logger.Error("Message: {Message}\n\n Stack trace: {StackTrace}\n\n", ex.Message, ex.StackTrace);
            }
        }
    }
}
