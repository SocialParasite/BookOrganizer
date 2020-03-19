using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using BookOrganizer.Domain;
using BookOrganizer.UI.WPFCore.DialogServiceManager;
using Prism.Commands;
using Serilog;

namespace BookOrganizer.UI.WPFCore.ViewModels.Statistics
{
    public class AnnualBookStatisticsInRangeReportViewModel : ViewModelBase, IReport
    {
        private readonly ILogger logger;
        private readonly IBookStatisticsYearRangeLookupDataService lookupService;
        private readonly IDialogService dialogService;

        private List<AnnualBookStatisticsInRangeReport> reportData;
        private int selectedBeginYear;
        private int selectedEndYear;

        public AnnualBookStatisticsInRangeReportViewModel(IBookStatisticsYearRangeLookupDataService lookupService,
                                                          ILogger logger,
                                                          IDialogService dialogService)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.lookupService = lookupService ?? throw new ArgumentNullException(nameof(lookupService));
            this.dialogService = dialogService ?? throw new ArgumentNullException(nameof(dialogService));

            YearSelectionChangedCommand = new DelegateCommand(OnYearSelectionChangedExecute);

            Init();
        }

        public ICommand YearSelectionChangedCommand { get; set; }
        public string ReportName => "Books read during period report";
        public IEnumerable<int> YearsList { get; set; }
        public List<AnnualBookStatisticsInRangeReport> ReportData
        {
            get => reportData;
            set
            {
                reportData = value;
                OnPropertyChanged();
            }
        }

        public int SelectedBeginYear
        {
            get { return selectedBeginYear; }
            set { selectedBeginYear = value; OnPropertyChanged(); }
        }

        public int SelectedEndYear
        {
            get { return selectedEndYear; }
            set { selectedEndYear = value; OnPropertyChanged(); }
        }

        private async Task InitializeReportAsync(int? beginYear = null, int? endYear = null)
        {
            if (SelectedBeginYear == default)
            {
                SelectedBeginYear = beginYear ?? DateTime.Now.Year - 10;
            }
            if (SelectedEndYear == default)
            {
                SelectedEndYear = endYear ?? DateTime.Now.Year;
            }
            if (SelectedEndYear < SelectedBeginYear)
            {
                SelectedEndYear = SelectedBeginYear;
            }

            YearsList = PopulateYearsMenu();

            try
            {
                var temp = await lookupService.GetAnnualBookStatisticsInRangeReportAsync(SelectedBeginYear, SelectedEndYear);
                ReportData = new List<AnnualBookStatisticsInRangeReport>(temp);
            }
            catch (Exception ex)
            {
                var dialog = new NotificationViewModel("Exception", ex.Message);
                dialogService.OpenDialog(dialog);

                logger.Error("Exception: {Exception} Message: {Message}\n\n Stack trace: {StackTrace}\n\n", ex.GetType().Name, ex.Message, ex.StackTrace);
            }
        }

        private Task Init(int? beginYear = null, int? endYear = null)
            => InitializeReportAsync(beginYear, endYear);

        private void OnYearSelectionChangedExecute()
        {
            Init(SelectedBeginYear, SelectedEndYear);
        }

        private IEnumerable<int> PopulateYearsMenu()
        {
            for (int year = DateTime.Today.Year; year > 0; year--)
                yield return year;
        }
    }
}
