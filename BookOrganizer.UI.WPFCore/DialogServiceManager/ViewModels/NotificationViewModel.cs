using System.Windows.Input;
using Prism.Commands;

namespace BookOrganizer.UI.WPFCore.DialogServiceManager
{
    public class NotificationViewModel : BaseDialog<DialogResult>
    {
        public NotificationViewModel(string title, string message) : base(title, message)
        {
            OKCommand = new DelegateCommand<IDialogWindow>(OKExecute);
        }
        
        public ICommand OKCommand { get; set; }

        private void OKExecute(IDialogWindow window)
        {
            CloseDialogWithResult(window, DialogResult.Undefined);
        }
    }
}
