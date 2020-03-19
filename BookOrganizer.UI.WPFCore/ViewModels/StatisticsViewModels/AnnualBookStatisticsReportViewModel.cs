using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Threading.Tasks;
using System.Windows.Input;
using BookOrganizer.Domain;
using BookOrganizer.UI.WPFCore.DialogServiceManager;
using Prism.Commands;
using Serilog;

namespace BookOrganizer.UI.WPFCore.ViewModels.Statistics
{
    public class AnnualBookStatisticsReportViewModel : ViewModelBase, IReport
    {
        private readonly ILogger logger;
        private readonly IAnnualBookStatisticsLookupDataService lookupService;
        private readonly IDialogService dialogService;

        private List<AnnualBookStatisticsReport> reportData;
        private int selectedYear;

        public AnnualBookStatisticsReportViewModel(IAnnualBookStatisticsLookupDataService lookupService,
                                                   ILogger logger,
                                                   IDialogService dialogService)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.lookupService = lookupService ?? throw new ArgumentNullException(nameof(lookupService));
            this.dialogService = dialogService ?? throw new ArgumentNullException(nameof(dialogService));

            YearSelectionChangedCommand = new DelegateCommand(OnYearSelectionChangedExecute);
            SelectedYear = DateTime.Now.Year;

            Init();
        }

        public ICommand YearSelectionChangedCommand { get; set; }
        public string ReportName => "Annual books read report";
        public IEnumerable<int> YearsList { get; set; }

        public List<AnnualBookStatisticsReport> ReportData
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
            get { return selectedYear; }
            set { selectedYear = value; OnPropertyChanged(); }
        }

        private IEnumerable<int> PopulateYearsMenu()
        {
            for (int year = DateTime.Today.Year; year > 0; year--)
                yield return year;
        }

        private Task Init(int? year = null)
            => InitializeRepositoryAsync(year);

        private async Task InitializeRepositoryAsync(int? year = null)
        {
            YearsList = PopulateYearsMenu();

            try
            {
                var temp = await lookupService.GetAnnualBookStatisticsReportAsync(year);
                ReportData = new List<AnnualBookStatisticsReport>(temp);
            }
            catch(SqlNullValueException ex)
            {
                var details = $"No statistics found for year {SelectedYear}";
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
    }
}
