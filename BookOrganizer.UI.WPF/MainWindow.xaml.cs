using BookOrganizer.Data.SqlServer;
using BookOrganizer.UI.WPF.ViewModels;
using System.Windows;

namespace BookOrganizer.UI.WPF
{
    public partial class MainWindow : Window
    {
        private readonly MainViewModel viewModel;

        public MainWindow(MainViewModel mainViewModel)
        {
            InitializeComponent();
            viewModel = mainViewModel;
            DataContext = viewModel;
        }
    }
}
