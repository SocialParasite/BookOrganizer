using System;
using System.Collections.ObjectModel;
using Autofac.Features.Indexed;
using BookOrganizer.UI.WPFCore.ViewModels.Statistics;

namespace BookOrganizer.UI.WPFCore.ViewModels
{
    public class BookStatisticsViewModel : ViewModelBase, ISelectedViewModel
    {
        private IReport selectedReport;
        private readonly IIndex<string, IReport> viewModelCreator;

        public BookStatisticsViewModel(IIndex<string, IReport> viewModelCreator)
        {
            this.viewModelCreator = viewModelCreator ?? throw new ArgumentNullException(nameof(viewModelCreator));

            InitializeView();
        }

        public IReport SelectedReport
        {
            get { return selectedReport; }
            set { selectedReport = value; OnPropertyChanged(); }
        }

        public ObservableCollection<IReport> Reports { get; set; }

        private void InitializeView()
        {
            Reports = new ObservableCollection<IReport>();
            Reports.Add(viewModelCreator[nameof(AnnualBookStatisticsReportViewModel)]);
            Reports.Add(viewModelCreator[nameof(AnnualBookStatisticsInRangeReportViewModel)]);
            Reports.Add(viewModelCreator[nameof(MonthlyReadsReportViewModel)]);
            SelectedReport = Reports[0];
        }
    }
}
