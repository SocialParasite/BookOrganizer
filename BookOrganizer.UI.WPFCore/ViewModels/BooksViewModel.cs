using System;
using System.Collections.Generic;
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
            EntityCollection ??= new List<LookupItem>();
            Init().Await();
        }

        private async Task Init()
            => await Task.Run(InitializeRepositoryAsync);

        public override async Task InitializeRepositoryAsync()
        {

            try
            {
                items = await bookLookupDataService.GetBookLookupAsync(nameof(BookDetailViewModel));
                //EntityCollection = items.ToList();
                // TODO: below op freezes UI.
                EntityCollection = items
                    .OrderBy(b => b.DisplayMember
                                      .StartsWith("A ", StringComparison.OrdinalIgnoreCase)
                                  || b.DisplayMember.StartsWith("The ", StringComparison.OrdinalIgnoreCase)
                        ? b.DisplayMember.Substring(b.DisplayMember.IndexOf(" ", StringComparison.Ordinal) + 1)
                        : b.DisplayMember)
                    .ToList();
                //.CreateOrderedEnumerable();
            }
            catch (Exception ex)
            {
                var dialog = new NotificationViewModel("Exception", ex.Message);
                dialogService.OpenDialog(dialog);

                logger.Error("Message: {Message}\n\n Stack trace: {StackTrace}\n\n", ex.Message, ex.StackTrace);
            }
        }

        private IEnumerable<IOrderedEnumerable<LookupItem>> OrderEntityCollection()
        {
            yield return items
                .OrderBy(b => b.DisplayMember
                                  .StartsWith("A ", StringComparison.OrdinalIgnoreCase)
                              || b.DisplayMember.StartsWith("The ", StringComparison.OrdinalIgnoreCase)
                    ? b.DisplayMember.Substring(b.DisplayMember.IndexOf(" ", StringComparison.Ordinal) + 1)
                    : b.DisplayMember);
            //.ToEnumerable<LookupItem>();
        }
    }

    public static class TaskExtensions
    {
        public static Task Await(this Task task)
        {
            return task;
        }
    }
}