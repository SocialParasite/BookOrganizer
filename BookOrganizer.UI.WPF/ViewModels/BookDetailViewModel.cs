using BookOrganizer.Domain;
using BookOrganizer.UI.WPF.Events;
using BookOrganizer.UI.WPF.Lookups;
using BookOrganizer.UI.WPF.Repositories;
using Microsoft.Win32;
using Prism.Commands;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace BookOrganizer.UI.WPF.ViewModels
{
    public class BookDetailViewModel : BaseDetailViewModel<Book>, IBookDetailViewModel
    {
        readonly IEventAggregator eventAggregator;
        private readonly ILanguageLookupDataService languageLookupDataService;
        private readonly IPublisherLookupDataService publisherLookupDataService;
        private Book selectedBook;
        private SolidColorBrush highlightBrush;
        private Guid selectedBookId;
        private bool isNewReadDate;
        private DateTime newReadDate;
        private ObservableCollection<LookupItem> languages;
        private LookupItem selectedLanguage;
        private ObservableCollection<LookupItem> publishers;
        private LookupItem selectedPublisher;

        public BookDetailViewModel(IEventAggregator eventAggregator,
            IRepository<Book> booksRepo,
            ILanguageLookupDataService languageLookupDataService,
            IPublisherLookupDataService publisherLookupDataService)
            : base(eventAggregator)
        {
            this.eventAggregator = eventAggregator ?? throw new ArgumentNullException(nameof(eventAggregator));
            this.languageLookupDataService = languageLookupDataService 
                ?? throw new ArgumentNullException(nameof(languageLookupDataService));
            this.publisherLookupDataService = publisherLookupDataService
                ?? throw new ArgumentNullException(nameof(publisherLookupDataService));

            ShowSelectedBookCommand = new DelegateCommand<Guid?>(ShowSelectedBookExecute);
            HighlightMouseOverCommand = new DelegateCommand(HighlightMouseOverExecute);
            HighlightMouseLeaveCommand = new DelegateCommand(HighlightMouseLeaveExecute);
            AddNewReadDateCommand = new DelegateCommand(AddNewReadDateExecute);
            SetReadDateCommand = new DelegateCommand(SetReadDateExecute);
            AddBookCoverImageCommand = new DelegateCommand(AddBookCoverImageExecute);

            Repository = booksRepo ?? throw new ArgumentNullException(nameof(booksRepo));

            IsNewReadDate = false;
            NewReadDate = DateTime.Today;
            Languages = new ObservableCollection<LookupItem>();
            Publishers = new ObservableCollection<LookupItem>();
        }

        public ICommand ShowSelectedBookCommand { get; }
        public ICommand HighlightMouseLeaveCommand { get; }
        public ICommand HighlightMouseOverCommand { get; }
        public ICommand AddNewReadDateCommand { get; set; }
        public ICommand SetReadDateCommand { get; set; }
        public ICommand AddBookCoverImageCommand { get; set; }

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

        public ObservableCollection<LookupItem> Languages
        {
            get { return languages; }
            set { languages = value; }
        }

        public LookupItem SelectedLanguage
        {
            get { return selectedLanguage; }
            set { selectedLanguage = value; OnPropertyChanged(); }
        }

        public ObservableCollection<LookupItem> Publishers
        {
            get { return publishers; }
            set { publishers = value; }
        }

        public LookupItem SelectedPublisher
        {
            get { return selectedPublisher; }
            set { selectedPublisher = value; OnPropertyChanged(); }
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

        private void AddBookCoverImageExecute()
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Title = "Select a picture as a book cover";
            op.Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" +
                        "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
                        "Portable Network Graphic (*.png)|*.png";

            if (op.ShowDialog() == true)
            {
                var coverPic = new BitmapImage(new Uri(op.FileName));
                // TODO: testing...
                var coverPath = @"C:\\temp\\";

                SelectedItem.BookCoverPicture = coverPath + coverPic.UriSource.Segments.LastOrDefault();
            }
        }

        public async override Task LoadAsync(Guid id)
        {
            var book = await Repository.GetSelectedAsync(id) ?? null;

            SelectedItem = book;
            Id = id;
        }

        public async override void SwitchEditableStateExecute()
        {
            base.SwitchEditableStateExecute();

            Languages.Clear();

            foreach (var item in await GetLanguageList())
            {
                Languages.Add(item);
            }

            SelectedLanguage = Languages.FirstOrDefault(l => l.Id == SelectedItem.Language.Id);

            Publishers.Clear();

            foreach (var item in await GetPublisherList())
            {
                Publishers.Add(item);
            }
            SelectedPublisher = Publishers.FirstOrDefault(p => p.Id == SelectedItem.Publisher.Id);
        }

        private async Task<IEnumerable<LookupItem>> GetPublisherList()
        {
            return await publisherLookupDataService.GetPublisherLookupAsync();
        }

        private async Task<IEnumerable<LookupItem>> GetLanguageList()
        {
            return await languageLookupDataService.GetLanguageLookupAsync();
        }

    }
}
