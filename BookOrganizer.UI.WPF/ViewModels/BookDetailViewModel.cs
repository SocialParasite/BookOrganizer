﻿using BookOrganizer.Domain;
using BookOrganizer.UI.WPF.Events;
using BookOrganizer.UI.WPF.Lookups;
using BookOrganizer.UI.WPF.Repositories;
using BookOrganizer.UI.WPF.Services;
using Prism.Commands;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;

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
        private int releaseYear;
        private int pageCount;
        private int wordCount;
        private string iSBN;

        private Guid selectedPublisherId;
        private Guid selectedAuthorId;
        private Guid selectedSeriesId;

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

        [Required]
        [MinLength(1, ErrorMessage = "Books title should be at minimum 1 character long.")]
        [MaxLength(256, ErrorMessage = "Books title should be maximum of 256 characters long.")]
        public string Title
        {
            get => title;
            set
            {
                ValidatePropertyInternal(nameof(Title), value);
                title = value;
                OnPropertyChanged();
                TabTitle = value;
                ((DelegateCommand)SaveItemCommand).RaiseCanExecuteChanged();
                SelectedItem.Title = value;
            }
        }

        [Range(1, 2500, ErrorMessage = "Fairytales older than year 1 shall not be permitted")]
        public int ReleaseYear
        {
            get => releaseYear;
            set
            {
                ValidatePropertyInternal(nameof(ReleaseYear), value);
                releaseYear = value;
                OnPropertyChanged();
                ((DelegateCommand)SaveItemCommand).RaiseCanExecuteChanged();
                SelectedItem.ReleaseYear = value;
            }
        }

        [Range(1, 10_000)]
        public int PageCount
        {
            get => pageCount;
            set
            {
                ValidatePropertyInternal(nameof(PageCount), value);
                pageCount = value;
                OnPropertyChanged();
                ((DelegateCommand)SaveItemCommand).RaiseCanExecuteChanged();
                SelectedItem.PageCount = value;
            }
        }

        [Range(0, int.MaxValue)]
        public int WordCount
        {
            get => wordCount;
            set
            {
                ValidatePropertyInternal(nameof(WordCount), value);
                wordCount = value;
                OnPropertyChanged();
                ((DelegateCommand)SaveItemCommand).RaiseCanExecuteChanged();
                SelectedItem.WordCount = value;
            }
        }

        [MaxLength(13)]
        public string ISBN
        {
            get => iSBN;
            set
            {
                ValidatePropertyInternal(nameof(ISBN), value);
                if (SelectedItem.ValidateIsbn(value))
                {
                    iSBN = value;
                    OnPropertyChanged();
                    ((DelegateCommand)SaveItemCommand).RaiseCanExecuteChanged();
                    SelectedItem.ISBN = value;
                }
                else
                    throw new ArgumentOutOfRangeException(nameof(ISBN), "Isbn must be valid or empty.");
            }
        }

        private void HighlightMouseLeaveExecute()
            => HighlightBrush = Brushes.White;

        private void HighlightMouseOverExecute()
            => HighlightBrush = Brushes.LightSkyBlue;

        private void SetReadDateExecute()
        {
            var newReadDate = new BooksReadDate { ReadDate = NewReadDate };

            if (!SelectedItem.ReadDates.Any(d => d.ReadDate == newReadDate.ReadDate))
            {
                SelectedItem.ReadDates.Add(newReadDate);
                SetChangeTracker();
            }
        }

        private async void AddBookCoverImageExecute()
        {
            SelectedItem.BookCoverPicturePath = FileExplorerService.BrowsePicture() ?? SelectedItem.BookCoverPicturePath;
            await LoadAsync(this.Id);
        }

        public async override Task LoadAsync(Guid id)
        {
            SelectedItem = await Repository.GetSelectedAsync(id) ?? null;

            Id = id;

            if (Id != Guid.Parse("00000000-0000-0000-0000-000000000000"))
            {
                TabTitle = SelectedItem.Title;
                Title = SelectedItem.Title;
                WordCount = SelectedItem.WordCount;
                ReleaseYear = SelectedItem.ReleaseYear;
                ISBN = SelectedItem.ISBN;
                PageCount = SelectedItem.PageCount;
            }
            else
                this.SwitchEditableStateExecute();

            SetDefaultBookCoverIfNoneSet();
            SetDefaultBookTitleIfNoneSet();
            InitiliazeSelectedLanguageIfNoneSet();
            InitializeSelectedPublisherIfNoneSet();
            SetBooksSelectedReleaseYear();

            SelectedItem.PropertyChanged += (s, e) =>
            {
                SetChangeTracker();
            };

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
            await InitializeAuthorCollection();

            ReleaseYear = SelectedReleaseYear;

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
                var removedAuthor = await (Repository as IBookRepository).GetBookAuthorById((Guid)authorId);
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

                SetChangeTracker();
            }
        }

        private async void AddBookAuthorExecute(LookupItem lookupItem)
        {
            if (lookupItem != null)
            {
                var addedAuthor = await (Repository as IBookRepository).GetBookAuthorById(lookupItem.Id);

                SelectedItem.AuthorsLink.Add(new BookAuthors
                {
                    AuthorId = addedAuthor.Id,
                    BookId = SelectedItem.Id,
                    Book = SelectedItem,
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
            if (SelectedItem.ReadDates.Any(d => d.ReadDate == readDate))
            {
                var deletedReadDate = SelectedItem.ReadDates.First(rd => rd.ReadDate == readDate);
                SelectedItem.ReadDates.Remove(deletedReadDate);

                SetChangeTracker();
            }
        }

        private void OnReleaseYearSelectionChangedExecute()
            => ReleaseYear = SelectedReleaseYear;

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
    }
}
