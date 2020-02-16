using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using BookOrganizer.Data.DA;
using BookOrganizer.Domain;
using Serilog;

namespace BookOrganizer.UI.WPFCore.ViewModels
{
    public class BookStatisticsViewModel : ViewModelBase, ISelectedViewModel
    {
        private readonly ILogger logger;
        private readonly BookStatisticsLookupDataService lookupService;
        private List<AnnualBookStatisticsReport> reportData;
        private int rowCount;

        public BookStatisticsViewModel(BookStatisticsLookupDataService lookupService, ILogger logger)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.lookupService = lookupService ?? throw new ArgumentNullException(nameof(lookupService));

            Init();
        }

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

        private Task Init()
            => InitializeRepositoryAsync();

        public async Task InitializeRepositoryAsync()
        {
            try
            {
                var temp = await lookupService.GetAnnualBookStatisticsReportAsync();
                ReportData = new List<AnnualBookStatisticsReport>(temp);
                RowCount = ReportData.Count;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                logger.Error("Message: {Message}\n\n Stack trace: {StackTrace}\n\n", ex.Message, ex.StackTrace);
            }
        }
    }
}
