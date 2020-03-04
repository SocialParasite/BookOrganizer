using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using BookOrganizer.Domain;
using BookOrganizer.Domain.Services;
using BookOrganizer.UI.WPFCore.Events;
using BookOrganizer.UI.WPFCore.Services;
using BookOrganizer.UI.WPFCore.Wrappers;
using Prism.Commands;
using Prism.Events;
using Serilog;

namespace BookOrganizer.UI.WPFCore.ViewModels
{
    public class BookDetailViewModel : BaseDetailViewModel<Book, BookWrapper>
    {
        private BookWrapper selectedItem;
        private SolidColorBrush highlightBrush;
        private DateTime newReadDate;
        private LookupItem selectedLanguage;
        private LookupItem selectedPublisher;
        private LookupItem selectedAuthor;
        private Guid selectedPublisherId;
        private Guid selectedAuthorId;
        private Guid selectedSeriesId;
        private int selectedReleaseYear;
        private string newFormatName;
        private string newGenreName;

        public BookDetailViewModel(IEventAggregator eventAggregator,
                                     ILogger logger,
                                     IDomainService<Book> domainService)
            : base(eventAggregator, logger, domainService)
        {
            HighlightMouseOverCommand = new DelegateCommand(HighlightMouseOverExecute);
            HighlightMouseLeaveCommand = new DelegateCommand(HighlightMouseLeaveExecute);
            SetReadDateCommand = new DelegateCommand(SetReadDateExecute);
            AddBookCoverImageCommand = new DelegateCommand(AddBookCoverImageExecute);
            AddAuthorAsABookAuthorCommand = new DelegateCommand<LookupItem>(AddBookAuthorExecute);
            AddNewAuthorCommand = new DelegateCommand(OnAddNewAuthorExecute);
            AddNewPublisherCommand = new DelegateCommand(OnAddNewPublisherExecute);
            AddNewLanguageCommand = new DelegateCommand(OnAddNewLanguageExecute);
            RemoveAuthorAsABookAuthorCommand = new DelegateCommand<Guid?>(RemoveAuthorExecute);
            LanguageSelectionChangedCommand = new DelegateCommand(OnLanguageSelectionChangedExecute);
            PublisherSelectionChangedCommand = new DelegateCommand(OnPublisherSelectionChangedExecute);
            RemoveDateAsABookReadDateCommand = new DelegateCommand<DateTime?>(OnRemoveDateAsABookReadDateExecute);
            ReleaseYearSelectionChangedCommand = new DelegateCommand(OnReleaseYearSelectionChangedExecute);
            ShowSelectedPublisherCommand
                = new DelegateCommand<Guid?>(OnShowSelectedPublisherExecute, OnShowSelectedPublisherCanExecute);
            ShowSelectedAuthorCommand = new DelegateCommand<Guid?>(OnShowSelectedAuthorExecute, OnShowSelectedAuthorCanExecute);
            ShowSelectedSeriesCommand = new DelegateCommand<Guid?>(OnShowSelectedSeriesExecute, OnShowSelectedSeriesCanExecute);
            BookFormatSelectionChangedCommand = new DelegateCommand<LookupItem>(OnBookFormatSelectionChangedExecute);
            BookGenreSelectionChangedCommand = new DelegateCommand<LookupItem>(OnBookGenreSelectionChangedExecute);
            AddNewFormatCommand = new DelegateCommand<string>(OnAddNewFormatExecute, OnAddNewFormatCanExecute)
                .ObservesProperty(() => NewFormatName);
            AddNewGenreCommand = new DelegateCommand<string>(OnAddNewGenreExecute, OnAddNewGenreCanExecute)
                .ObservesProperty(() => NewGenreName);

            SelectedItem = new BookWrapper(domainService.CreateItem(), domainService);

            NewReadDate = DateTime.Today;
            Languages = new ObservableCollection<LookupItem>();
            Publishers = new ObservableCollection<LookupItem>();
            Authors = new ObservableCollection<LookupItem>();
            AllBookFormats = new ObservableCollection<Tuple<LookupItem, bool>>();
            AllBookGenres = new ObservableCollection<Tuple<LookupItem, bool>>();

            YearsList = PopulateYearsMenu();
        }

        public ICommand HighlightMouseLeaveCommand { get; }
        public ICommand HighlightMouseOverCommand { get; }
        public ICommand SetReadDateCommand { get; set; }
        public ICommand AddBookCoverImageCommand { get; }
        public ICommand AddNewAuthorCommand { get; }
        public ICommand AddNewPublisherCommand { get; }
        public ICommand AddNewLanguageCommand { get; }
        public ICommand AddAuthorAsABookAuthorCommand { get; }
        public ICommand RemoveAuthorAsABookAuthorCommand { get; }
        public ICommand LanguageSelectionChangedCommand { get; }
        public ICommand PublisherSelectionChangedCommand { get; }
        public ICommand RemoveDateAsABookReadDateCommand { get; }
        public ICommand ReleaseYearSelectionChangedCommand { get; }
        public ICommand ShowSelectedPublisherCommand { get; }
        public ICommand ShowSelectedAuthorCommand { get; }
        public ICommand ShowSelectedSeriesCommand { get; }
        public ICommand BookFormatSelectionChangedCommand { get; }
        public ICommand BookGenreSelectionChangedCommand { get; }
        public ICommand AddNewFormatCommand { get; }
        public ICommand AddNewGenreCommand { get; }

        public Guid SelectedPublisherId
        {
            get => selectedPublisherId;
            set
            {
                selectedPublisherId = value;
                OnPropertyChanged();
                if (selectedPublisherId != Guid.Empty)
                {
                    eventAggregator.GetEvent<OpenItemMatchingSelectedPublisherIdEvent<Guid>>()
                                   .Publish(SelectedPublisherId);
                }
            }
        }

        public Guid SelectedAuthorId
        {
            get => selectedAuthorId;
            set
            {
                selectedAuthorId = value;
                OnPropertyChanged();
                if (selectedAuthorId != Guid.Empty)
                {
                    eventAggregator.GetEvent<OpenItemMatchingSelectedAuthorIdEvent<Guid>>()
                                   .Publish(SelectedAuthorId);
                }
            }
        }

        public Guid SelectedSeriesId
        {
            get => selectedSeriesId;
            set
            {
                selectedSeriesId = value;
                OnPropertyChanged();
                if (selectedSeriesId != Guid.Empty)
                {
                    eventAggregator.GetEvent<OpenItemMatchingSelectedSeriesIdEvent<Guid>>()
                                   .Publish(SelectedSeriesId);
                }
            }
        }

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

        public ObservableCollection<Tuple<LookupItem, bool>> AllBookFormats { get; set; }

        public ObservableCollection<Tuple<LookupItem, bool>> AllBookGenres { get; set; }

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

        public override BookWrapper SelectedItem
        {
            get { return selectedItem; }
            set
            {
                selectedItem = value ?? throw new ArgumentNullException(nameof(SelectedItem));
                OnPropertyChanged();
            }
        }

        public string NewFormatName
        {
            get { return newFormatName; }
            set { newFormatName = value; OnPropertyChanged(); }
        }

        public string NewGenreName
        {
            get { return newGenreName; }
            set { newGenreName = value; OnPropertyChanged(); }
        }

        private void HighlightMouseLeaveExecute()
           => HighlightBrush = Brushes.White;

        private void HighlightMouseOverExecute()
            => HighlightBrush = Brushes.LightSkyBlue;

        private void SetReadDateExecute()
        {
            var newReadDate = new BooksReadDate { ReadDate = NewReadDate };

            if (!SelectedItem.Model.ReadDates.Any(d => d.ReadDate == newReadDate.ReadDate))
            {
                SelectedItem.Model.ReadDates.Add(newReadDate);
                SetChangeTracker();
            }
        }

        private void AddBookCoverImageExecute()
        {
            SelectedItem.BookCoverPicturePath = FileExplorerService.BrowsePicture() ?? SelectedItem.BookCoverPicturePath;
        }

        public async override Task LoadAsync(Guid id)
        {
            try
            {
                var book = await domainService.Repository.GetSelectedAsync(id) ?? new Book();

                SelectedItem = CreateWrapper(book);

                SelectedItem.PropertyChanged += (s, e) =>
                {
                    if (!HasChanges)
                    {
                        HasChanges = domainService.Repository.HasChanges();
                    }
                    if (e.PropertyName == nameof(SelectedItem.HasErrors))
                    {
                        ((DelegateCommand)SaveItemCommand).RaiseCanExecuteChanged();
                    }
                    if (e.PropertyName == nameof(SelectedItem.Title))
                    {
                        TabTitle = SelectedItem.Title;
                    }
                };
                ((DelegateCommand)SaveItemCommand).RaiseCanExecuteChanged();

                Id = id;

                if (Id != default)
                {
                    TabTitle = SelectedItem.Title;
                }
                else
                {
                    this.SwitchEditableStateExecute();
                }

                SetDefaultBookCoverIfNoneSet();
                SetDefaultBookTitleIfNoneSet();
                InitiliazeSelectedLanguageIfNoneSet();
                InitializeSelectedPublisherIfNoneSet();
                SetBooksSelectedReleaseYear();
                InitializeAllBookFormats();
                InitializeAllBookGenres();

                void SetDefaultBookCoverIfNoneSet()
                {
                    if (SelectedItem.BookCoverPicturePath is null)
                        SelectedItem.BookCoverPicturePath = FileExplorerService.GetImagePath();
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
                        if (SelectedItem.Model.Language != null)
                        {
                            SelectedLanguage =
                                new LookupItem
                                {
                                    Id = SelectedItem.Model.Language.Id,
                                    DisplayMember = SelectedItem.Model.Language is null
                                    ? new Language().LanguageName = ""
                                    : SelectedItem.Model.Language.LanguageName
                                };
                        }
                    }
                }

                void InitializeSelectedPublisherIfNoneSet()
                {
                    if (SelectedPublisher is null)
                    {
                        if (SelectedItem.Model.Publisher != null)
                        {
                            SelectedPublisher =
                                new LookupItem
                                {
                                    Id = SelectedItem.Model.Publisher.Id,
                                    DisplayMember = SelectedItem.Model.Publisher is null
                                    ? new Publisher().Name = ""
                                    : SelectedItem.Model.Publisher.Name
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

                void InitializeAllBookFormats()
                {
                    if (SelectedItem.Model.FormatLink != null)
                    {
                        if (!AllBookFormats.Any())
                        {
                            AllBookFormats.Clear();

                            foreach (var item in SelectedItem.Model.FormatLink)
                            {
                                var newLookupItem = new LookupItem { Id = item.Format.Id, DisplayMember = item.Format.Name };
                                AllBookFormats.Add((newLookupItem, true).ToTuple());
                            }
                        }
                    }
                }

                void InitializeAllBookGenres()
                {
                    if (SelectedItem.Model.GenreLink != null)
                    {
                        if (!AllBookGenres.Any())
                        {
                            AllBookGenres.Clear();

                            foreach (var item in SelectedItem.Model.GenreLink)
                            {
                                var newLookupItem = new LookupItem { Id = item.Genre.Id, DisplayMember = item.Genre.Name };
                                AllBookGenres.Add((newLookupItem, true).ToTuple());
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                logger.Error("Message: {Message}\n\n Stack trace: {StackTrace}\n\n", ex.Message, ex.StackTrace);
            }
        }

        public override async void SwitchEditableStateExecute()
        {
            base.SwitchEditableStateExecute();

            await InitializeLanguageCollection();
            await InitializePublisherCollection();
            await InitializeAuthorCollection();
            await InitializeAllBookFormatsCollection();
            await InitializeAllBookGenresCollection();

            SelectedItem.ReleaseYear = SelectedReleaseYear == 0 ? 1 : SelectedReleaseYear;

            async Task InitializeLanguageCollection()
            {
                if (!Languages.Any())
                {
                    Languages.Clear();

                    foreach (var item in await GetLanguageList())
                    {
                        Languages.Add(item);
                    }

                    if (SelectedItem.Model.Language != null)
                        SelectedLanguage = Languages.FirstOrDefault(l => l.Id == SelectedItem.Model.Language.Id);
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

                    if (SelectedItem.Model.Publisher != null)
                        SelectedPublisher = Publishers.FirstOrDefault(p => p.Id == SelectedItem.Model.Publisher.Id);
                }
            }

            async Task InitializeAuthorCollection()
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
            async Task InitializeAllBookFormatsCollection()
            {
                if (UserMode.Item1 == false)
                {
                    foreach (var item in await GetBookFormatList())
                    {
                        if (AllBookFormats.Any(i => i.Item1.Id == item.Id))
                        {
                            continue;
                        }
                        AllBookFormats.Add((item, false).ToTuple());
                    }
                }
                else
                {
                    var tempBookFormatsCollection = new ObservableCollection<Tuple<LookupItem, bool>>();
                    foreach (var item in AllBookFormats)
                    {
                        if (item.Item2 == true)
                        {
                            tempBookFormatsCollection.Add(item);
                        }
                    }
                    AllBookFormats.Clear();

                    foreach (var item in tempBookFormatsCollection)
                    {
                        AllBookFormats.Add(item);
                    }
                }
            }

            async Task InitializeAllBookGenresCollection()
            {
                if (UserMode.Item1 == false)
                {
                    foreach (var item in await GetBookGenreList())
                    {
                        if (AllBookGenres.Any(i => i.Item1.Id == item.Id))
                        {
                            continue;
                        }
                        AllBookGenres.Add((item, false).ToTuple());
                    }
                }
                else
                {
                    var tempBookGenresCollection = new ObservableCollection<Tuple<LookupItem, bool>>();
                    foreach (var item in AllBookGenres)
                    {
                        if (item.Item2 == true)
                        {
                            tempBookGenresCollection.Add(item);
                        }
                    }
                    AllBookGenres.Clear();

                    foreach (var item in tempBookGenresCollection)
                    {
                        AllBookGenres.Add(item);
                    }
                }
            }

        private async Task<IEnumerable<LookupItem>> GetPublisherList()
                    => await (domainService as BookService).publisherLookupDataService.GetPublisherLookupAsync(nameof(PublisherDetailViewModel));

        private async Task<IEnumerable<LookupItem>> GetLanguageList()
            => await (domainService as BookService).languageLookupDataService.GetLanguageLookupAsync(nameof(LanguageDetailViewModel));

        private async Task<IEnumerable<LookupItem>> GetAuthorList()
            => await (domainService as BookService).authorLookupDataService.GetAuthorLookupAsync(nameof(AuthorDetailViewModel));

        private async Task<IEnumerable<LookupItem>> GetBookFormatList()
        => await (domainService as BookService).formatLookupDataService.GetFormatLookupAsync(nameof(FormatDetailViewModel));

        private async Task<IEnumerable<LookupItem>> GetBookGenreList()
        => await (domainService as BookService).genreLookupDataService.GetGenreLookupAsync(nameof(GenreDetailViewModel));

        private async void RemoveAuthorExecute(Guid? authorId)
        {
            if (authorId != null)
            {
                var removedAuthor = await (domainService.Repository as IBookRepository).GetBookAuthorById((Guid)authorId);
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

                var removedAuthorAsLookupItem = SelectedItem.Model.AuthorsLink.First(al => al.Author.Id == authorId);
                SelectedItem.Model.AuthorsLink.Remove(removedAuthorAsLookupItem);

                SetChangeTracker();
            }
        }

        private async void AddBookAuthorExecute(LookupItem lookupItem)
        {
            if (lookupItem != null)
            {
                var addedAuthor = await (domainService.Repository as IBookRepository).GetBookAuthorById(lookupItem.Id);

                SelectedItem.Model.AuthorsLink.Add(new BookAuthors
                {
                    AuthorId = addedAuthor.Id,
                    BookId = SelectedItem.Id,
                    Book = SelectedItem.Model,
                    Author = addedAuthor
                });

                Authors.Remove(lookupItem);

                SetChangeTracker();
            }
        }

        private void OnLanguageSelectionChangedExecute()
        {
            if (SelectedLanguage != null && Languages.Any())
            {
                SelectedItem.LanguageId = SelectedLanguage.Id;
                SetChangeTracker();
            }
        }

        private void OnPublisherSelectionChangedExecute()
        {
            if (SelectedPublisher != null && Publishers.Any())
            {
                SelectedItem.PublisherId = SelectedPublisher.Id;
                SetChangeTracker();
            }
        }

        private async void OnBookFormatSelectionChangedExecute(LookupItem lookupItem)
        {
            if (AllBookFormats.Any(bf => bf.Item1.Id == lookupItem.Id && bf.Item2 == false))
            {
                AllBookFormats.Remove(AllBookFormats.Where(bf => bf.Item1.Id == lookupItem.Id).Single());
                AllBookFormats.Add((lookupItem, true).ToTuple());

                var newFormat = await (domainService.Repository as IBookRepository).GetBookFormatById(lookupItem.Id);

                SelectedItem.Model.FormatLink.Add(new BooksFormats
                {
                    FormatId = newFormat.Id,
                    Format = newFormat,
                    Book = SelectedItem.Model,
                    BookId = SelectedItem.Id
                });
            }
            else if (AllBookFormats.Any(bf => bf.Item1.Id == lookupItem.Id && bf.Item2 == true))
            {
                AllBookFormats.Remove(AllBookFormats.Where(bf => bf.Item1.Id == lookupItem.Id).Single());
                AllBookFormats.Add((lookupItem, false).ToTuple());

                var item = SelectedItem.Model.FormatLink.Single(f => f.FormatId == lookupItem.Id);
                SelectedItem.Model.FormatLink.Remove(item);
            }
            SetChangeTracker();
        }

        private async void OnBookGenreSelectionChangedExecute(LookupItem lookupItem)
        {
            if (AllBookGenres.Any(bg => bg.Item1.Id == lookupItem.Id && bg.Item2 == false))
            {
                AllBookGenres.Remove(AllBookGenres.Where(bg => bg.Item1.Id == lookupItem.Id).Single());
                AllBookGenres.Add((lookupItem, true).ToTuple());

                var newGenre = await (domainService.Repository as IBookRepository).GetBookGenreById(lookupItem.Id);

                SelectedItem.Model.GenreLink.Add(new BookGenres
                {
                    GenreId = newGenre.Id,
                    Genre = newGenre,
                    Book = SelectedItem.Model,
                    BookId = SelectedItem.Id
                });
            }
            else if (AllBookGenres.Any(bg => bg.Item1.Id == lookupItem.Id && bg.Item2 == true))
            {
                AllBookGenres.Remove(AllBookGenres.Where(bg => bg.Item1.Id == lookupItem.Id).Single());
                AllBookGenres.Add((lookupItem, false).ToTuple());

                var item = SelectedItem.Model.GenreLink.Single(g => g.GenreId == lookupItem.Id);
                SelectedItem.Model.GenreLink.Remove(item);
            }
            SetChangeTracker();
        }

        private bool OnShowSelectedAuthorCanExecute(Guid? id)
            => (id is null || id == Guid.Empty) ? false : true;

        private void OnShowSelectedAuthorExecute(Guid? id)
            => SelectedAuthorId = (Guid)id;

        private bool OnShowSelectedPublisherCanExecute(Guid? id)
            => (id is null || id == Guid.Empty) ? false : true;

        private void OnShowSelectedPublisherExecute(Guid? id)
            => SelectedPublisherId = (Guid)id;

        private void OnShowSelectedSeriesExecute(Guid? id)
            => SelectedSeriesId = (Guid)id;

        private bool OnShowSelectedSeriesCanExecute(Guid? id)
            => (id is null || id == Guid.Empty) ? false : true;

        private void OnRemoveDateAsABookReadDateExecute(DateTime? readDate)
        {
            if (SelectedItem.Model.ReadDates.Any(d => d.ReadDate == readDate))
            {
                var deletedReadDate = SelectedItem.Model.ReadDates.First(rd => rd.ReadDate == readDate);
                SelectedItem.Model.ReadDates.Remove(deletedReadDate);

                SetChangeTracker();
            }
        }

        private void OnReleaseYearSelectionChangedExecute()
            => SelectedItem.ReleaseYear = SelectedReleaseYear;

        private IEnumerable<int> PopulateYearsMenu()
        {
            for (int year = DateTime.Today.Year; year > 0; year--)
                yield return year;
        }
        private void OnAddNewAuthorExecute()
        {
            eventAggregator.GetEvent<OpenDetailViewEvent>()
                           .Publish(new OpenDetailViewEventArgs
                           {
                               Id = new Guid(),
                               ViewModelName = nameof(AuthorDetailViewModel)
                           });

        }
        private void OnAddNewPublisherExecute()
        {
            eventAggregator.GetEvent<OpenDetailViewEvent>()
                           .Publish(new OpenDetailViewEventArgs
                           {
                               Id = new Guid(),
                               ViewModelName = nameof(PublisherDetailViewModel)
                           });
        }
        private void OnAddNewLanguageExecute()
        {
            eventAggregator.GetEvent<OpenDetailViewEvent>()
                           .Publish(new OpenDetailViewEventArgs
                           {
                               Id = new Guid(),
                               ViewModelName = nameof(LanguageDetailViewModel)
                           });
        }

        public override BookWrapper CreateWrapper(Book entity)
        {
            var wrapper = new BookWrapper(entity, domainService);
            return wrapper;
        }

        private bool OnAddNewFormatCanExecute(string arg)
        {
            if (arg is null)
            {
                return false;
            }
            var test = AllBookFormats.Any(f => f.Item1.DisplayMember.Equals(arg, StringComparison.InvariantCultureIgnoreCase));
            return !test;
        }

        private bool OnAddNewGenreCanExecute(string arg)
        {
            if (arg is null)
            {
                return false;
            }
            return !AllBookGenres.Any(f => f.Item1.DisplayMember.Equals(arg, StringComparison.InvariantCultureIgnoreCase));
        }

        private void OnAddNewGenreExecute(string genre)
        {
            AddNewBookGenreToCollection(genre);
        }

        private async Task AddNewBookGenreToCollection(string genre)
        {
            var newGenre = new Genre();
            newGenre.Name = genre;

            await (domainService as BookService).AddNewBookGenre(newGenre);
            await InitializeAllBookGenresCollection();
        }

        private void OnAddNewFormatExecute(string format)
        {
            AddNewBookFormatToCollection(format);
        }

        private async Task AddNewBookFormatToCollection(string format)
        {
            var newFormat = new Format();
            newFormat.Name = format;

            await (domainService as BookService).AddNewBookFormat(newFormat);

            await InitializeAllBookFormatsCollection();
        }
    }
}
