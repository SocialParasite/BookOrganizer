using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BookOrganizer.Data.DA;
using BookOrganizer.Domain;
using BookOrganizer.UI.WPFCore.DialogServiceManager;
using Serilog;

namespace BookOrganizer.UI.WPFCore.ViewModels.Statistics
{
    public class AnnualBookStatisticsInRangeReportViewModel : ViewModelBase, IReport
    {
        private readonly ILogger logger;
        private readonly ReportLookupDataService lookupService;
        private readonly IDialogService dialogService;

        private List<AnnualBookStatisticsInRangeReport> reportData;
        private int rowCount;

        public AnnualBookStatisticsInRangeReportViewModel(ReportLookupDataService lookupService, 
                                                          ILogger logger, 
                                                          IDialogService dialogService)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.lookupService = lookupService ?? throw new ArgumentNullException(nameof(lookupService));
            this.dialogService = dialogService ?? throw new ArgumentNullException(nameof(dialogService));

            Init();
        }

        public string ReportName => "Books read during period report";

        public List<AnnualBookStatisticsInRangeReport> ReportData
        {
            get => reportData;
            set
            {
                reportData = value;
                OnPropertyChanged();
            }
        }

        public int RowCount
        {
            get { return rowCount; }
            private set { rowCount = value; OnPropertyChanged(); }
        }

        public async Task InitializeRepositoryAsync()
        {
            try
            {
                var temp = await lookupService.GetAnnualBookStatisticsInRangeReportAsync(2019, 2020);
                ReportData = new List<AnnualBookStatisticsInRangeReport>(temp);
                RowCount = ReportData.Count;
            }
            catch (Exception ex)
            {
                var dialog = new NotificationViewModel("Exception", ex.Message);
                dialogService.OpenDialog(dialog);

                logger.Error("Exception: {Exception} Message: {Message}\n\n Stack trace: {StackTrace}\n\n", ex.GetType().Name, ex.Message, ex.StackTrace);
            }
        }

        private Task Init()
            => InitializeRepositoryAsync();
    }
}
