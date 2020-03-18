using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BookOrganizer.Data.DA;
using BookOrganizer.Domain;
using BookOrganizer.UI.WPFCore.DialogServiceManager;
using Serilog;

namespace BookOrganizer.UI.WPFCore.ViewModels.Statistics
{
    public class AnnualBookStatisticsReportViewModel : ViewModelBase, IReport
    {
        private readonly ILogger logger;
        private readonly ReportLookupDataService lookupService;
        private readonly IDialogService dialogService;

        private List<AnnualBookStatisticsReport> reportData;
        private int rowCount;

        public AnnualBookStatisticsReportViewModel(ReportLookupDataService lookupService, 
                                                   ILogger logger, 
                                                   IDialogService dialogService)
        {
                this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
                this.lookupService = lookupService ?? throw new ArgumentNullException(nameof(lookupService));
                this.dialogService = dialogService ?? throw new ArgumentNullException(nameof(dialogService));

                Init();
        }

        public string ReportName => "Annual books read report";

        public List<AnnualBookStatisticsReport> ReportData
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
                var temp = await lookupService.GetAnnualBookStatisticsReportAsync(2019);
                ReportData = new List<AnnualBookStatisticsReport>(temp);
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
