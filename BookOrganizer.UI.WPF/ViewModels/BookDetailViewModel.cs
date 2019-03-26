using BookOrganizer.Domain;
using BookOrganizer.UI.WPF.Events;
using BookOrganizer.UI.WPF.Lookups;
using BookOrganizer.UI.WPF.Repositories;
using BookOrganizer.UI.WPF.Services;
using Microsoft.Win32;
using Prism.Commands;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace BookOrganizer.UI.WPF.ViewModels
{
    public class BookDetailViewModel : BaseDetailViewModel<Book>, IBookDetailViewModel
    {
        private readonly ILanguageLookupDataService languageLookupDataService;
        private readonly IPublisherLookupDataService publisherLookupDataService;
        private readonly IAuthorLookupDataService authorLookupDataService;
        private SolidColorBrush highlightBrush;
        private DateTime newReadDate;
        private LookupItem selectedLanguage;
        private LookupItem selectedPublisher;
        private LookupItem selectedAuthor;
        private int selectedReleaseYear;
        private string title;

        public BookDetailViewModel(IEventAggregator eventAggregator,
            IMetroDialogService metroDialogService,
            IRepository<Book> booksRepo,
            ILanguageLookupDataService languageLookupDataService,
            IPublisherLookupDataService publisherLookupDataService,
            IAuthorLookupDataService authorLookupDataService)
            : base(eventAggregator, metroDialogService)
        {
            this.languageLookupDataService = languageLookupDataService
                ?? throw new ArgumentNullException(nameof(languageLookupDataService));
            this.publisherLookupDataService = publisherLookupDataService
                ?? throw new ArgumentNullException(nameof(publisherLookupDataService));
            this.authorLookupDataService = authorLookupDataService
                ?? throw new ArgumentNullException(nameof(authorLookupDataService));

            HighlightMouseOverCommand = new DelegateCommand(HighlightMouseOverExecute);
            HighlightMouseLeaveCommand = new DelegateCommand(HighlightMouseLeaveExecute);
            SetReadDateCommand = new DelegateCommand(SetReadDateExecute);
            AddBookCoverImageCommand = new DelegateCommand(AddBookCoverImageExecute);
            AddAuthorAsABookAuthorCommand = new DelegateCommand<LookupItem>(AddBookAuthorExecute);
            RemoveAuthorAsABookAuthorCommand = new DelegateCommand<Guid?>(RemoveAuthorExecute);
            LanguageSelectionChangedCommand = new DelegateCommand(OnLanguageSelectionChangedExecute);
            PublisherSelectionChangedCommand = new DelegateCommand(OnPublisherSelectionChangedExecute);
            RemoveDateAsABookReadDateCommand = new DelegateCommand<DateTime?>(OnRemoveDateAsABookReadDateExecute);
            ReleaseYearSelectionChangedCommand = new DelegateCommand(OnReleaseYearSelectionChangedExecute);

            Repository = booksRepo ?? throw new ArgumentNullException(nameof(booksRepo));

            NewReadDate = DateTime.Today;
            Languages = new ObservableCollection<LookupItem>();
            Publishers = new ObservableCollection<LookupItem>();
            Authors = new ObservableCollection<LookupItem>();

            SelectedItem = new Book();
            YearsList = PopulateYearsMenu();
        }

        public ICommand HighlightMouseLeaveCommand { get; }
        public ICommand HighlightMouseOverCommand { get; }
        public ICommand SetReadDateCommand { get; set; }
        public ICommand AddBookCoverImageCommand { get; set; }
        public ICommand AddAuthorAsABookAuthorCommand { get; set; }
        public ICommand RemoveAuthorAsABookAuthorCommand { get; set; }
        public ICommand LanguageSelectionChangedCommand { get; set; }
        public ICommand PublisherSelectionChangedCommand { get; set; }
        public ICommand RemoveDateAsABookReadDateCommand { get; set; }
        public ICommand ReleaseYearSelectionChangedCommand { get; set; }

        public SolidColorBrush HighlightBrush
        {
            get { return highlightBrush; }
            set { highlightBrush = value; OnPropertyChanged(); }
        }

        public ObservableCollection<LookupItem> Languages { get; set; }

        public LookupItem SelectedLanguage
        {
            get { return selectedLanguage; }
            set { selectedLanguage = value; OnPropertyChanged(); }
        }

        public ObservableCollection<LookupItem> Publishers { get; set; }

        public LookupItem SelectedPublisher
        {
            get { return selectedPublisher; }
            set { selectedPublisher = value; OnPropertyChanged(); }
        }

        public ObservableCollection<LookupItem> Authors { get; set; }

        public LookupItem SelectedAuthor
        {
            get { return selectedAuthor; }
            set { selectedAuthor = value; OnPropertyChanged(); }
        }

        public DateTime NewReadDate
        {
            get { return newReadDate; }
            set { newReadDate = value; OnPropertyChanged(); }
        }

        public IEnumerable<int> YearsList { get; set; }

        public int SelectedReleaseYear
        {
            get { return selectedReleaseYear; }
            set { selectedReleaseYear = value; OnPropertyChanged(); }
        }

        public string Title
        {
            get { return title; }
            set { title = value; OnPropertyChanged(); TabTitle = value; SelectedItem.Title = value; }
        }

        private void HighlightMouseLeaveExecute()
            => HighlightBrush = Brushes.White;

        private void HighlightMouseOverExecute()
            => HighlightBrush = Brushes.LightSkyBlue;

        private void SetReadDateExecute()
        {
            var newReadDate = new BooksReadDate { ReadDate = NewReadDate };

            if (!SelectedItem.ReadDates.Any(d => d.ReadDate == newReadDate.ReadDate))
                SelectedItem.ReadDates.Add(newReadDate);
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
            SelectedItem = await Repository.GetSelectedAsync(id) ?? null;

            Id = id;

            if (Id != Guid.Parse("00000000-0000-0000-0000-000000000000"))
            {
                TabTitle = SelectedItem.Title;
                Title = SelectedItem.Title;
            }

            SetDefaultBookCoverIfNoneSet();
            SetDefaultBookTitleIfNoneSet();
            InitiliazeSelectedLanguageIfNoneSet();
            InitializeSelectedPublisherIfNoneSet();
            SetBooksSelectedReleaseYear();

            SelectedItem.PropertyChanged += (s, e) =>
            {
                if (!HasChanges)
                {
                    HasChanges = Repository.HasChanges();
                }
            };

            void SetDefaultBookCoverIfNoneSet()
            {
                if (SelectedItem.BookCoverPicture is null)
                    SelectedItem.BookCoverPicture =
                        $"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase).Substring(6)}\\placeholder.png";
            }

            void SetDefaultBookTitleIfNoneSet()
            {
                if (SelectedItem.Title == "" || SelectedItem.Title is null)
                    SelectedItem.Title = "Book Title";
            }

            void InitiliazeSelectedLanguageIfNoneSet()
            {
                if (SelectedLanguage is null)
                {
                    if (SelectedItem.Language != null)
                    {
                        SelectedLanguage =
                            new LookupItem
                            {
                                Id = SelectedItem.Language.Id,
                                DisplayMember = SelectedItem.Language is null
                                ? new Language().LanguageName = ""
                                : SelectedItem.Language.LanguageName
                            };
                    }
                }
            }

            void InitializeSelectedPublisherIfNoneSet()
            {
                if (SelectedPublisher is null)
                {
                    if (SelectedItem.Publisher != null)
                    {
                        SelectedPublisher =
                            new LookupItem
                            {
                                Id = SelectedItem.Publisher.Id,
                                DisplayMember = SelectedItem.Publisher is null
                                ? new Publisher().Name = ""
                                : SelectedItem.Publisher.Name
                            };
                    }
                }
            }

            void SetBooksSelectedReleaseYear()
            {
                SelectedReleaseYear = SelectedItem.ReleaseYear == 0
                    ? DateTime.Today.Year
                    : SelectedItem.ReleaseYear;
            }
        }

        public async override void SwitchEditableStateExecute()
        {
            base.SwitchEditableStateExecute();

            await InitializeLanguageCollection();
            await InitializePublisherCollection();
            await InitalizeAuthorCollection();

            SelectedItem.ReleaseYear = SelectedReleaseYear;

            async Task InitializeLanguageCollection()
            {
                if (!Languages.Any())
                {
                    Languages.Clear();

                    foreach (var item in await GetLanguageList())
                    {
                        Languages.Add(item);
                    }

                    if (SelectedItem.Language != null)
                        SelectedLanguage = Languages.FirstOrDefault(l => l.Id == SelectedItem.Language.Id);
                }
            }

            async Task InitializePublisherCollection()
            {
                if (!Publishers.Any())
                {
                    Publishers.Clear();

                    foreach (var item in await GetPublisherList())
                    {
                        Publishers.Add(item);
                    }

                    if (SelectedItem.Publisher != null)
                        SelectedPublisher = Publishers.FirstOrDefault(p => p.Id == SelectedItem.Publisher.Id);
                }
            }

            async Task InitalizeAuthorCollection()
            {
                if (!Authors.Any())
                {
                    Authors.Clear();

                    foreach (var item in await GetAuthorList())
                    {
                        Authors.Add(item);
                    }
                }
            }
        }

        private async Task<IEnumerable<LookupItem>> GetPublisherList()
            => await publisherLookupDataService.GetPublisherLookupAsync();

        private async Task<IEnumerable<LookupItem>> GetLanguageList()
            => await languageLookupDataService.GetLanguageLookupAsync();

        private async Task<IEnumerable<LookupItem>> GetAuthorList()
            => await authorLookupDataService.GetAuthorLookupAsync();

        private async void RemoveAuthorExecute(Guid? authorId)
        {
            if (authorId != null)
            {
                var removedAuthor = await authorLookupDataService.GetAuthorById((Guid)authorId);
                Authors.Add(
                    new LookupItem
                    {
                        Id = (Guid)authorId,
                        DisplayMember = $"{removedAuthor.LastName}, {removedAuthor.FirstName}"
                    });

                var temporaryAuthorCollection = new ObservableCollection<LookupItem>();
                temporaryAuthorCollection.AddRange(Authors.OrderBy(a => a.DisplayMember));
                Authors.Clear();

                foreach (var item in temporaryAuthorCollection)
                {
                    Authors.Add(item);
                }

                var removedAuthorAsLookupItem = SelectedItem.AuthorsLink.First(al => al.Author.Id == authorId);
                SelectedItem.AuthorsLink.Remove(removedAuthorAsLookupItem);
            }
        }

        private async void AddBookAuthorExecute(LookupItem lookupItem)
        {
            if (lookupItem != null)
            {
                var addedAuthor = await authorLookupDataService.GetAuthorById(lookupItem.Id);
                SelectedItem.AuthorsLink.Add(new BookAuthors
                {
                    AuthorId = addedAuthor.Id,
                    BookId = SelectedItem.Id,
                    Book = SelectedItem,
                    Author = addedAuthor
                });

                Authors.Remove(lookupItem);
            }
        }

        private void OnLanguageSelectionChangedExecute()
        {
            if (SelectedLanguage != null && Languages.Any())
                SelectedItem.LanguageId = SelectedLanguage.Id;
        }

        private void OnPublisherSelectionChangedExecute()
        {
            if (SelectedPublisher != null && Publishers.Any())
                SelectedItem.PublisherId = SelectedPublisher.Id;
        }

        private void OnRemoveDateAsABookReadDateExecute(DateTime? readDate)
        {
            if (SelectedItem.ReadDates.Any(d => d.ReadDate == readDate))
            {
                var deletedReadDate = SelectedItem.ReadDates.First(rd => rd.ReadDate == readDate);
                SelectedItem.ReadDates.Remove(deletedReadDate);
            }
        }

        private void OnReleaseYearSelectionChangedExecute()
        {
            SelectedItem.ReleaseYear = SelectedReleaseYear;
        }

        private IEnumerable<int> PopulateYearsMenu()
        {
            for (int year = DateTime.Today.Year; year > 0; year--)
                yield return year;
        }
    }
}
