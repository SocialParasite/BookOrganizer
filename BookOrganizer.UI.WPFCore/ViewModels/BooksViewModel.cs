using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using BookOrganizer.Domain;
using Prism.Events;
using Serilog;

namespace BookOrganizer.UI.WPFCore.ViewModels
{
    public class BooksViewModel : BaseViewModel<Book>
    {
        private readonly IBookLookupDataService bookLookupDataService;

        public BooksViewModel(IEventAggregator eventAggregator,
                              IBookLookupDataService bookLookupDataService,
                              ILogger logger)
            : base(eventAggregator, logger)
        {
            this.bookLookupDataService = bookLookupDataService ?? throw new ArgumentNullException(nameof(bookLookupDataService));

            Init();

            ViewModelType = nameof(BookDetailViewModel);
        }

        private Task Init()
            => InitializeRepositoryAsync();

        public override async Task InitializeRepositoryAsync()
        {
            try
            {
                Items = await bookLookupDataService.GetBookLookupAsync(nameof(BookDetailViewModel));

                EntityCollection = Items
                    .OrderBy(b => b.DisplayMember
                        .StartsWith("A ", StringComparison.OrdinalIgnoreCase)
                        || b.DisplayMember.StartsWith("The ", StringComparison.OrdinalIgnoreCase)
                        ? b.DisplayMember.Substring(b.DisplayMember.IndexOf(" ") + 1)
                        : b.DisplayMember)
                    .ToList();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
                logger.Error("Message: {Message}\n\n Stack trace: {StackTrace}\n\n", ex.Message, ex.StackTrace);
            }
        }
    }
}