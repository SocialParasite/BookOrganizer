using BookOrganizer.Domain;
using BookOrganizer.UI.WPF.Lookups;
using Prism.Events;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BookOrganizer.UI.WPF.ViewModels
{
    public class BooksViewModel : BaseViewModel<Book>, IBooksViewModel
    {
        private readonly IBookLookupDataService bookLookupDataService;

        public BooksViewModel(IEventAggregator eventAggregator,
                              IBookLookupDataService bookLookupDataService)
            : base(eventAggregator)
        {
            this.bookLookupDataService = bookLookupDataService ?? throw new ArgumentNullException(nameof(bookLookupDataService));

            InitializeRepositoryAsync();
        }

        public ICommand BookTitleLabelMouseLeftButtonUpCommand { get; }


        public override async Task InitializeRepositoryAsync()
        {
            Items = await bookLookupDataService.GetBookLookupAsync();

            EntityCollection = Items.OrderBy(b => b.DisplayMember).ToList();
        }
    }
}
