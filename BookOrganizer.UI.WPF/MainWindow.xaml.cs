using BookOrganizer.UI.WPF.ViewModels;
using MahApps.Metro.Controls;

namespace BookOrganizer.UI.WPF
{
    public partial class MainWindow : MetroWindow
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
