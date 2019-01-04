using BookOrganizer.Domain;
using BookOrganizer.UI.WPF.Events;
using BookOrganizer.UI.WPF.Repositories;
using Prism.Commands;
using Prism.Events;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;

namespace BookOrganizer.UI.WPF.ViewModels
{
    public class BookDetailViewModel : BaseDetailViewModel<Book>, IBookDetailViewModel
    {
        readonly IEventAggregator eventAggregator;
        readonly MainViewModel mainViewModel;

        private Book selectedBook;
        private SolidColorBrush highlightBrush;
        private Guid selectedBookId;

        public BookDetailViewModel(IEventAggregator eventAggregator,
            IRepository<Book> booksRepo,
            MainViewModel mainViewModel)
            : base(eventAggregator)
        {
            this.mainViewModel = mainViewModel ?? throw new ArgumentNullException(nameof(mainViewModel));
            this.eventAggregator = eventAggregator ?? throw new ArgumentNullException(nameof(eventAggregator));

            ShowSelectedBookCommand = new DelegateCommand<Guid?>(ShowSelectedBookExecute);
            HighlightMouseOverCommand = new DelegateCommand(HighlightMouseOverExecute);
            HighlightMouseLeaveCommand = new DelegateCommand(HighlightMouseLeaveExecute);
            SwitchEditableStateCommand = new DelegateCommand(SwitchEditableStateExecute);
            Repository = booksRepo ?? throw new ArgumentNullException(nameof(booksRepo));

            UserMode = (true, 0, Brushes.LightGray, false).ToTuple();
        }

        public ICommand ShowSelectedBookCommand { get; }
        public ICommand HighlightMouseLeaveCommand { get; }
        public ICommand HighlightMouseOverCommand { get; }
        public ICommand SwitchEditableStateCommand { get; set; }

        public Book SelectedBook
        {
            get { return selectedBook; }
            set { selectedBook = value ?? throw new ArgumentNullException(nameof(SelectedBook)); }
        }

        public SolidColorBrush HighlightBrush
        {
            get { return highlightBrush; }
            set { highlightBrush = value; OnPropertyChanged(); }
        }

        public Guid SelectedBookId
        {
            get => selectedBookId;
            set
            {
                selectedBookId = value;
                OnPropertyChanged();
                if (selectedBookId != Guid.Empty)
                {
                    eventAggregator.GetEvent<OpenItemMatchingSelectedBookIdEvent<Guid>>()
                                   .Publish(selectedBookId);
                }
            }
        }

        private Tuple<bool, int, SolidColorBrush, bool> userMode;

        public Tuple<bool, int, SolidColorBrush, bool> UserMode
        {
            get => userMode;
            set { userMode = value; OnPropertyChanged(); }
        }

        public async override Task LoadAsync(Guid id)
        {
            var book = await Repository.GetSelectedAsync(id) ?? null;

            SelectedItem = book;
            Id = id;
        }

        private void ShowSelectedBookExecute(Guid? id)
            => SelectedBookId = (Guid)id;

        private void HighlightMouseLeaveExecute()
            => HighlightBrush = Brushes.White;

        private void HighlightMouseOverExecute()
            => HighlightBrush = Brushes.LightSkyBlue;

        private void SwitchEditableStateExecute()
        {
            if (UserMode.Item2 == 0)
                UserMode = (!UserMode.Item1, 1, Brushes.LightGreen, !UserMode.Item4).ToTuple();
            else
                UserMode = (!UserMode.Item1, 0, Brushes.LightGray, !UserMode.Item4).ToTuple();

        }
    }
}
