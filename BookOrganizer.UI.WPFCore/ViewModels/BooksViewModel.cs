using BookOrganizer.DA;
using BookOrganizer.Domain;
using Prism.Events;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BookOrganizer.UI.WPFCore.ViewModels
{
    public class BooksViewModel : BaseViewModel<Book>
    {
        private readonly IBookLookupDataService bookLookupDataService;

        public BooksViewModel(IEventAggregator eventAggregator,
                              IBookLookupDataService bookLookupDataService)
            : base(eventAggregator)
        {
            this.bookLookupDataService = bookLookupDataService ?? throw new ArgumentNullException(nameof(bookLookupDataService));

            Init();
        }

        public Task Init() 
            => InitializeRepositoryAsync();

        public override async Task InitializeRepositoryAsync()
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
    }
}