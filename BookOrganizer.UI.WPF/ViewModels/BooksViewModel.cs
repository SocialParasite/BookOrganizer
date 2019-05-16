using BookOrganizer.Domain;
using BookOrganizer.UI.WPF.Events;
using BookOrganizer.UI.WPF.Lookups;
using Prism.Commands;
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

            BookTitleLabelMouseLeftButtonUpCommand
                = new DelegateCommand<Guid?>(OnBookTitleLabelMouseLeftButtonUpExecute, OnBookTitleLabelMouseLeftButtonUpCanExecute);
            //AddNewBookCommand = new DelegateCommand(OnAddNewBookExecute);

            InitializeRepositoryAsync();
        }

        public ICommand BookTitleLabelMouseLeftButtonUpCommand { get; }
        //public ICommand AddNewBookCommand { get; }


        public override async Task InitializeRepositoryAsync()
        {
            Items = await bookLookupDataService.GetBookLookupAsync();

            EntityCollection = Items.OrderBy(b => b.DisplayMember).ToList();
        }

        //private void OnAddNewBookExecute()
        //{
        //    eventAggregator.GetEvent<OpenDetailViewEvent>()
        //                           .Publish(new OpenDetailViewEventArgs
        //                           {
        //                               Id = new Guid(),
        //                               ViewModelName = nameof(BookDetailViewModel)
        //                           });
        //}

        private void OnBookTitleLabelMouseLeftButtonUpExecute(Guid? id)
            => OnOpenBookMatchingSelectedId((Guid)id);

        private bool OnBookTitleLabelMouseLeftButtonUpCanExecute(Guid? id)
            => (id is null || id == Guid.Empty) ? false : true;

        private void OnOpenBookMatchingSelectedId(Guid id)
        {
            eventAggregator.GetEvent<OpenDetailViewEvent>()
                                   .Publish(new OpenDetailViewEventArgs
                                   {
                                       Id = id,
                                       ViewModelName = nameof(BookDetailViewModel)
                                   });
        }
    }
}
