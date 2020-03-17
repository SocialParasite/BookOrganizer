using System.Windows;
using System.Windows.Media.Effects;

namespace BookOrganizer.UI.WPFCore.DialogServiceManager
{
    public class DialogService : IDialogService
    {
        public T OpenDialog<T>(BaseDialog<T> viewModel)
        {
            var window = new DialogWindow();
            window.Owner = Application.Current.MainWindow;
            //window.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            window.Left = Application.Current.MainWindow.Left;
            window.Width = Application.Current.MainWindow.ActualWidth - 16;
            window.Height = Application.Current.MainWindow.ActualHeight / 3;
            //Application.Current.MainWindow
            window.DataContext = viewModel;
            Application.Current.MainWindow.Effect = new BlurEffect();
            window.ShowDialog();
            Application.Current.MainWindow.Effect = null;
            return viewModel.DialogResult;
        }
    }
}
