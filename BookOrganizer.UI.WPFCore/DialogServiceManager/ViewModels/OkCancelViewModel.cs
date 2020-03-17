using System.Windows.Input;
using Prism.Commands;

namespace BookOrganizer.UI.WPFCore.DialogServiceManager
{
    public class OkCancelViewModel : BaseDialog<DialogResult>
    {
        public OkCancelViewModel(string title, string message) : base(title, message)
        {
            OKCommand = new DelegateCommand<IDialogWindow>(OKExecute);
            CancelCommand = new DelegateCommand<IDialogWindow>(CancelExecute);
        }

        public ICommand OKCommand { get; set; }
        public ICommand CancelCommand { get; set; }

        private void OKExecute(IDialogWindow window)
        {
            CloseDialogWithResult(window, DialogResult.Yes);
        }

        private void CancelExecute(IDialogWindow window)
        {
            CloseDialogWithResult(window, DialogResult.No);
        }
    }
}
