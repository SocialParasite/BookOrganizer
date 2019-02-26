using MahApps.Metro.Controls.Dialogs;
using System.Threading.Tasks;

namespace BookOrganizer.UI.WPF.Services
{
    public interface IMetroDialogService
    {
        Task ShowInfoDialogAsync(string text);
        Task<MessageDialogResult> ShowOkCancelDialogAsync(string text, string title);
    }
}