using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System.Threading.Tasks;

namespace BookOrganizer.UI.WPF.Services
{
    public class MetroDialogService : IMetroDialogService
    {
        private MetroWindow MetroWindow => (MetroWindow)App.Current.MainWindow;

        public async Task<MessageDialogResult> ShowOkCancelDialogAsync(string text, string title)
        {
            var result = await MetroWindow.ShowMessageAsync(title, text, MessageDialogStyle.AffirmativeAndNegative);

            return result == MessageDialogResult.Affirmative
              ? MessageDialogResult.Affirmative
              : MessageDialogResult.Canceled;
        }

        public async Task ShowInfoDialogAsync(string text)
            => await MetroWindow.ShowMessageAsync("Info", text);
    }
}
