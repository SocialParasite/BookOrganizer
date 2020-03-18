using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using BookOrganizer.Data.DA;
using BookOrganizer.UI.WPFCore.DialogServiceManager;
using BookOrganizer.UI.WPFCore.ViewModels.Statistics;
using Prism.Commands;
using Serilog;

namespace BookOrganizer.UI.WPFCore.ViewModels
{
    public class BookStatisticsViewModel : ViewModelBase, ISelectedViewModel
    {
        private IReport selectedReport;
        private readonly AnnualBookStatisticsReportViewModel vm;

        public BookStatisticsViewModel(AnnualBookStatisticsReportViewModel vm)
        {
            this.vm = vm ?? throw new ArgumentNullException(nameof(vm));

            ReportSelectionChangedCommand = new DelegateCommand(OnReportSelectionChangedExecute);

            InitializeView();
        }

        public ICommand ReportSelectionChangedCommand { get; }

        private void OnReportSelectionChangedExecute()
        {
            throw new NotImplementedException();
        }

        public IReport SelectedReport
        {
            get { return selectedReport; }
            set { selectedReport = value; OnPropertyChanged(); }
        }

        public ObservableCollection<IReport> Reports { get; set; }

        public void InitializeView()
        {
            SelectedReport = vm;
            Reports = new ObservableCollection<IReport>();
            Reports.Add(vm);
        }
    }
}
