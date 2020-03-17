namespace BookOrganizer.UI.WPFCore.DialogServiceManager
{
    public interface IDialogService
    {
        T OpenDialog<T>(BaseDialog<T> viewModel);

    }
}
