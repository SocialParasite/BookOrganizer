namespace BookOrganizer.UI.WPFCore.DialogServiceManager
{
    public abstract class BaseDialog<T>
    {
        public string Title { get; set; }
        public string Message { get; set; }
        public T DialogResult { get; set; }

        public BaseDialog() : this(string.Empty, string.Empty) { }

        public BaseDialog(string title) : this(title, string.Empty) { }

        public BaseDialog(string title, string message)
        {
            Title = title;
            Message = message;
        }

        public void CloseDialogWithResult(IDialogWindow dialog, T result)
        {
            DialogResult = result;

            if (dialog != null)
            {
                dialog.DialogResult = true;
            }
        }
    }
}
