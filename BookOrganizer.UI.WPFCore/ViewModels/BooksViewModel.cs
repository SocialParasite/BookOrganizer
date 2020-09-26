using System;
using System.Linq;
using System.Threading.Tasks;
using BookOrganizer.Domain;
using BookOrganizer.UI.WPFCore.DialogServiceManager;
using Prism.Events;
using Serilog;

namespace BookOrganizer.UI.WPFCore.ViewModels
{
    public class BooksViewModel : BaseViewModel<Book>
    {
        private readonly IBookLookupDataService bookLookupDataService;

        public BooksViewModel(IEventAggregator eventAggregator,
                              IBookLookupDataService bookLookupDataService,
                              ILogger logger,
                              IDialogService dialogService)
            : base(eventAggregator, logger, dialogService)
        {
            this.bookLookupDataService = bookLookupDataService ?? throw new ArgumentNullException(nameof(bookLookupDataService));

            ViewModelType = nameof(BookDetailViewModel);
            Init().Await();
        }

        private async Task Init()
            => await Task.Run(InitializeRepositoryAsync);

        public override async Task InitializeRepositoryAsync()
        {

            try
            {
                items = await bookLookupDataService.GetBookLookupAsync(nameof(BookDetailViewModel));

                // TODO: below op freezes UI.
                EntityCollection = items
                    .OrderBy(b => b.DisplayMember
                        .StartsWith("A ", StringComparison.OrdinalIgnoreCase)
                        || b.DisplayMember.StartsWith("The ", StringComparison.OrdinalIgnoreCase)
                        ? b.DisplayMember.Substring(b.DisplayMember.IndexOf(" ", StringComparison.Ordinal) + 1)
                        : b.DisplayMember)
                    .ToList();
            }
            catch (Exception ex)
            {
                var dialog = new NotificationViewModel("Exception", ex.Message);
                dialogService.OpenDialog(dialog);

                logger.Error("Message: {Message}\n\n Stack trace: {StackTrace}\n\n", ex.Message, ex.StackTrace);
            }
        }
    }

    public static class TaskExtensions
    {
        public static async void Await(this Task task)
        {
            await task;
        }
    }
}