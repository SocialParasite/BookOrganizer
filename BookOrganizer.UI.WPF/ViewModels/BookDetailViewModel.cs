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

        private Book selectedBook;
        private SolidColorBrush highlightBrush;
        private Guid selectedBookId;
        private bool isNewReadDate;
        private DateTime newReadDate;

        public BookDetailViewModel(IEventAggregator eventAggregator,
            IRepository<Book> booksRepo)
            : base(eventAggregator)
        {
            this.eventAggregator = eventAggregator ?? throw new ArgumentNullException(nameof(eventAggregator));

            ShowSelectedBookCommand = new DelegateCommand<Guid?>(ShowSelectedBookExecute);
            HighlightMouseOverCommand = new DelegateCommand(HighlightMouseOverExecute);
            HighlightMouseLeaveCommand = new DelegateCommand(HighlightMouseLeaveExecute);
            AddNewReadDateCommand = new DelegateCommand(AddNewReadDateExecute);
            SetReadDateCommand = new DelegateCommand(SetReadDateExecute);

            Repository = booksRepo ?? throw new ArgumentNullException(nameof(booksRepo));

            IsNewReadDate = false;
            NewReadDate = DateTime.Today;
        }

        public ICommand ShowSelectedBookCommand { get; }
        public ICommand HighlightMouseLeaveCommand { get; }
        public ICommand HighlightMouseOverCommand { get; }
        public ICommand AddNewReadDateCommand { get; set; }
        public ICommand SetReadDateCommand { get; set; }

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


        public bool IsNewReadDate
        {
            get { return isNewReadDate; }
            set { isNewReadDate = value; OnPropertyChanged(); }
        }

        public DateTime NewReadDate
        {
            get { return newReadDate; }
            set { newReadDate = value; OnPropertyChanged(); }
        }

        private void ShowSelectedBookExecute(Guid? id)
            => SelectedBookId = (Guid)id;

        private void HighlightMouseLeaveExecute()
            => HighlightBrush = Brushes.White;

        private void HighlightMouseOverExecute()
            => HighlightBrush = Brushes.LightSkyBlue;

        private void AddNewReadDateExecute() 
            => IsNewReadDate = true;

        private void SetReadDateExecute()
        {
            var test = new BooksReadDate { Book = SelectedItem, ReadDate = NewReadDate };
            SelectedItem.ReadDates.Add(test);
        }

        public async override Task LoadAsync(Guid id)
        {
            var book = await Repository.GetSelectedAsync(id) ?? null;

            SelectedItem = book;
            Id = id;
        }
    }
}
