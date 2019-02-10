﻿using BookOrganizer.Domain;
using BookOrganizer.UI.WPF.Events;
using BookOrganizer.UI.WPF.Lookups;
using BookOrganizer.UI.WPF.Repositories;
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
        readonly IEventAggregator eventAggregator;
        private readonly ILanguageLookupDataService languageLookupDataService;
        private readonly IPublisherLookupDataService publisherLookupDataService;
        private readonly IAuthorLookupDataService authorLookupDataService;
        private Book selectedBook;
        private SolidColorBrush highlightBrush;
        private Guid selectedBookId;
        private DateTime newReadDate;
        private ObservableCollection<LookupItem> languages;
        private LookupItem selectedLanguage;
        private ObservableCollection<LookupItem> publishers;
        private LookupItem selectedPublisher;
        private ObservableCollection<LookupItem> authors;
        private LookupItem selectedAuthor;

        public BookDetailViewModel(IEventAggregator eventAggregator,
            IRepository<Book> booksRepo,
            ILanguageLookupDataService languageLookupDataService,
            IPublisherLookupDataService publisherLookupDataService,
            IAuthorLookupDataService authorLookupDataService)
            : base(eventAggregator)
        {
            this.eventAggregator = eventAggregator ?? throw new ArgumentNullException(nameof(eventAggregator));
            this.languageLookupDataService = languageLookupDataService
                ?? throw new ArgumentNullException(nameof(languageLookupDataService));
            this.publisherLookupDataService = publisherLookupDataService
                ?? throw new ArgumentNullException(nameof(publisherLookupDataService));
            this.authorLookupDataService = authorLookupDataService
                ?? throw new ArgumentNullException(nameof(authorLookupDataService));

            ShowSelectedBookCommand = new DelegateCommand<Guid?>(ShowSelectedBookExecute);
            HighlightMouseOverCommand = new DelegateCommand(HighlightMouseOverExecute);
            HighlightMouseLeaveCommand = new DelegateCommand(HighlightMouseLeaveExecute);
            SetReadDateCommand = new DelegateCommand(SetReadDateExecute);
            AddBookCoverImageCommand = new DelegateCommand(AddBookCoverImageExecute);
            AddAuthorAsABookAuthorCommand = new DelegateCommand<LookupItem>(AddBookAuthorExecute);
            RemoveAuthorAsABookAuthorCommand = new DelegateCommand<Guid?>(RemoveAuthorExecute);
            LanguageSelectionChangedCommand = new DelegateCommand(OnLanguageSelectionChangedExecute);
            PublisherSelectionChangedCommand = new DelegateCommand(OnPublisherSelectionChangedExecute);
            RemoveDateAsABookReadDateCommand = new DelegateCommand<DateTime?>(OnRemoveDateAsABookReadDateExecute);

            Repository = booksRepo ?? throw new ArgumentNullException(nameof(booksRepo));

            NewReadDate = DateTime.Today;
            Languages = new ObservableCollection<LookupItem>();
            Publishers = new ObservableCollection<LookupItem>();
            Authors = new ObservableCollection<LookupItem>();

            SelectedItem = new Book();
        }

        private void OnRemoveDateAsABookReadDateExecute(DateTime? readDate)
        {
            if (SelectedItem.ReadDates.Any(d => d.ReadDate == readDate))
                SelectedItem.ReadDates.Remove(SelectedItem.ReadDates.First(d => d.ReadDate == readDate));
        }

        public ICommand ShowSelectedBookCommand { get; }
        public ICommand HighlightMouseLeaveCommand { get; }
        public ICommand HighlightMouseOverCommand { get; }
        public ICommand SetReadDateCommand { get; set; }
        public ICommand AddBookCoverImageCommand { get; set; }
        public ICommand AddAuthorAsABookAuthorCommand { get; set; }
        public ICommand RemoveAuthorAsABookAuthorCommand { get; set; }
        public ICommand LanguageSelectionChangedCommand { get; set; }
        public ICommand PublisherSelectionChangedCommand { get; set; }
        public ICommand RemoveDateAsABookReadDateCommand { get; set; }

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

        public ObservableCollection<LookupItem> Authors
        {
            get { return authors; }
            set { authors = value; }
        }

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

        private void ShowSelectedBookExecute(Guid? id)
            => SelectedBookId = (Guid)id;

        private void HighlightMouseLeaveExecute()
            => HighlightBrush = Brushes.White;

        private void HighlightMouseOverExecute()
            => HighlightBrush = Brushes.LightSkyBlue;

        private void SetReadDateExecute()
        {
            var newReadDate = new BooksReadDate { Book = SelectedItem, ReadDate = NewReadDate };

            if(!SelectedItem.ReadDates.Any(d => d.ReadDate == newReadDate.ReadDate))
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
            var book = await Repository.GetSelectedAsync(id) ?? null;

            SelectedItem = book;
            Id = id;

            if (SelectedItem.BookCoverPicture is null)
                SelectedItem.BookCoverPicture =
                    $"{Path.GetDirectoryName((Assembly.GetExecutingAssembly().GetName().CodeBase)).Substring(6)}\\placeholder.png";

            if (SelectedLanguage is null)
            {
                SelectedLanguage =
                    new LookupItem
                    {
                        Id = SelectedItem.Language.Id,
                        DisplayMember = SelectedItem.Language.LanguageName
                    };
            }

            if (SelectedPublisher is null)
            {
                SelectedPublisher =
                    new LookupItem
                    {
                        Id = SelectedItem.Publisher.Id,
                        DisplayMember = SelectedItem.Publisher.Name
                    };
            }
        }

        public async override void SwitchEditableStateExecute()
        {
            base.SwitchEditableStateExecute();

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

            if (!Authors.Any())
            {
                Authors.Clear();

                foreach (var item in await GetAuthorList())
                {
                    Authors.Add(item);
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
                SelectedItem.AuthorsLink.Add(new BookAuthors { Author = addedAuthor, Book = SelectedItem });

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
    }
}
