using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Threading.Tasks;
using System.Windows.Input;
using BookOrganizer.Domain;
using BookOrganizer.Domain.DA_Interfaces.Reports;
using BookOrganizer.UI.WPFCore.DialogServiceManager;
using Prism.Commands;
using Serilog;

namespace BookOrganizer.UI.WPFCore.ViewModels.Statistics
{
    public class MonthlyReadsReportViewModel : ViewModelBase, IReport
    {
        private readonly ILogger logger;
        private readonly IMonthlyReadsLookupDataService lookupService;
        private readonly IDialogService dialogService;

        private List<MonthlyReadsReport> reportData;
        private int selectedYear;
        private Months selectedMonth;

        public MonthlyReadsReportViewModel(IMonthlyReadsLookupDataService lookupService,
                                                   ILogger logger,
                                                   IDialogService dialogService)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.lookupService = lookupService ?? throw new ArgumentNullException(nameof(lookupService));
            this.dialogService = dialogService ?? throw new ArgumentNullException(nameof(dialogService));

            YearSelectionChangedCommand = new DelegateCommand(OnYearSelectionChangedExecute);
            SelectedYear = DateTime.Now.Year;
            MonthSelectionChangedCommand = new DelegateCommand(OnMonthSelectionChangedExecute);
            SelectedMonth = (Months)DateTime.Now.Month;

            Init();
        }

        public ICommand YearSelectionChangedCommand { get; set; }
        public ICommand MonthSelectionChangedCommand { get; set; }

        public string ReportName => "Monthly reads report";
        public IEnumerable<int> YearsList { get; set; }
        public IEnumerable<string> MonthsList { get; set; }

        public List<MonthlyReadsReport> ReportData
        {
            get => reportData;
            set
            {
                reportData = value;
                OnPropertyChanged();
            }
        }

        public int SelectedYear
        {
            get => selectedYear;
            set { selectedYear = value; OnPropertyChanged(); }
        }

        public Months SelectedMonth
        {
            get => selectedMonth;
            set { selectedMonth = value; OnPropertyChanged(); }
        }

        private IEnumerable<int> PopulateYearsMenu()
        {
            for (int year = DateTime.Today.Year; year > 0; year--)
                yield return year;
        }

        private IEnumerable<string> PopulateMonthsMenu()
        {
            for (int i = 0; i < 12; i++)
            {
                yield return ((Months)i).ToString();
            }
        }

        private Task Init(int? year = null, int? month = null)
            => InitializeRepositoryAsync(year, month);

        private async Task InitializeRepositoryAsync(int? year = null, int? month = null)
        {
            YearsList = PopulateYearsMenu();
            MonthsList = PopulateMonthsMenu();
            month++;

            try
            {
                var monthlyReads = await lookupService.GetMonthlyReadsReportAsync(year, month);
                ReportData = new List<MonthlyReadsReport>(monthlyReads);
            }
            catch (SqlNullValueException ex)
            {
                var details = $"No statistics found for month {((Months)SelectedMonth + 1).ToString()} of year {SelectedYear}";
                var dialog = new NotificationViewModel("Error!", details);
                dialogService.OpenDialog(dialog);
                logger.Error("Exception: {Exception} Details: {Details} Message: {Message}\n\n Stack trace: {StackTrace}\n\n",
                    ex.GetType().Name, details, ex.Message, ex.StackTrace);
            }
            catch (Exception ex)
            {
                var dialog = new NotificationViewModel("Error!", ex.Message);
                dialogService.OpenDialog(dialog);
                logger.Error("Exception: {Exception} Message: {Message}\n\n Stack trace: {StackTrace}\n\n", ex.GetType().Name, ex.Message, ex.StackTrace);
            }
        }

        private void OnYearSelectionChangedExecute()
            => Init(SelectedYear);

        private void OnMonthSelectionChangedExecute()
            => Init(SelectedYear, (int)SelectedMonth);
    }

    public enum Months
    {
        January,
        February,
        March,
        April,
        May,
        June,
        July,
        August,
        September,
        October,
        November,
        December
    }
}
