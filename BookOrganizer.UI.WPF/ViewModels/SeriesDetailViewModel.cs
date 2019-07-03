using BookOrganizer.Data.Lookups;
using BookOrganizer.Data.Repositories;
using BookOrganizer.Domain;
using BookOrganizer.UI.WPF.Services;
using GongSolutions.Wpf.DragDrop;
using Prism.Commands;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace BookOrganizer.UI.WPF.ViewModels
{
    public class SeriesDetailViewModel : BaseDetailViewModel<Series>, IDropTarget
    {
        private readonly IBookLookupDataService bookLookupDataService;
        private string name;

        public SeriesDetailViewModel(IEventAggregator eventAggregator,
            IMetroDialogService metroDialogService,
            IRepository<Series> seriesRepo,
            IBookLookupDataService bookLookupDataService)
            : base(eventAggregator, metroDialogService)
        {
            Repository = seriesRepo ?? throw new ArgumentNullException(nameof(seriesRepo));

            AddSeriesPictureCommand = new DelegateCommand(OnAddSeriesPictureExecute);
            FilterBookListCommand = new DelegateCommand<string>(OnFilterBookListExecute);
            AddBookToSeriesCommand = new DelegateCommand<Guid?>(OnAddBookToSeriesExecute, OnAddBookToSeriesCanExecute);

            this.bookLookupDataService = bookLookupDataService;

            Books = new ObservableCollection<LookupItem>();
            AllBooks = new ObservableCollection<LookupItem>();

            SelectedItem = new Series();
        }

        public ICommand AddSeriesPictureCommand { get; }
        public ICommand FilterBookListCommand { get; }
        public ICommand AddBookToSeriesCommand { get; }

        public ObservableCollection<LookupItem> Books { get; set; }
        public ObservableCollection<LookupItem> AllBooks { get; set; }

        [Required]
        [MinLength(1, ErrorMessage = "Series name should be at minimum 1 character long.")]
        [MaxLength(256, ErrorMessage = "Series name should be maximum of 256 characters long.")]
        public string Name
        {
            get { return name; }
            set
            {
                ValidatePropertyInternal(nameof(Name), value);
                name = value;
                OnPropertyChanged();
                TabTitle = value;

                ((DelegateCommand)SaveItemCommand).RaiseCanExecuteChanged();

                SelectedItem.Name = value;
            }
        }

        public async override Task LoadAsync(Guid id)
        {
            SelectedItem = await Repository.GetSelectedAsync(id) ?? new Series();

            Id = id;

            if (Id != default)
            {
                TabTitle = SelectedItem.Name;
                Name = SelectedItem.Name;
            }
            else
                this.SwitchEditableStateExecute();

            await PopulateAllBooksCollection();
            SetDefaultSeriesPictureIfNoneSet();

            SelectedItem.PropertyChanged += (s, e) =>
            {
                SetChangeTracker();
            };

            void SetDefaultSeriesPictureIfNoneSet()
            {
                if (SelectedItem.PicturePath is null)
                    SelectedItem.PicturePath = FileExplorerService.GetImagePath();
            }

            async Task PopulateAllBooksCollection()
            {
                AllBooks.Clear();
                foreach (var item in await GetBookList())
                {
                    AllBooks.Add(item);
                }

            }
        }

        public override void SwitchEditableStateExecute()
        {
            base.SwitchEditableStateExecute();

            InitializeBookCollection();

            void InitializeBookCollection()
            {
                if (!Books.Any())
                {
                    var tempBookCollection = AllBooks.Where(item => !SelectedItem.BooksInSeries
                                                     .Any(x => x.Id == item.Id))
                                                     .OrderBy(b => b.DisplayMember);

                    PopulateBooksCollection(tempBookCollection);
                }
            }
        }

        public override void OnShowSelectedBookExecute(Guid? id)
        {
            if (UserMode.Item2 == Enums.DetailViewState.ViewMode)
            {
                base.OnShowSelectedBookExecute(id);
            }
            else
            {
                var book = SelectedItem.BooksInSeries.First(b => b.Id == id);
                SelectedItem.BooksInSeries.Remove(book);

                var sro = SelectedItem.SeriesReadOrder.First(b => b.BookId == id);
                SelectedItem.SeriesReadOrder.Remove(sro);

                Books.Add(new LookupItem { Id = book.Id, DisplayMember = book.Title, Picture = book.BookCoverPicturePath });

                var countOfBooksInSeries = SelectedItem.SeriesReadOrder.Count();
                SelectedItem.NumberOfBooks = countOfBooksInSeries;

                var inst = sro.Instalment;
                if (inst < ++countOfBooksInSeries)
                {
                    foreach (var item in SelectedItem.SeriesReadOrder)
                    {
                        if (item.Instalment > inst)
                            item.Instalment--;
                    }
                }

                RefreshSeriesReadOrder();

                SetChangeTracker();
            }

        }

        private void RefreshSeriesReadOrder()
        {
            var temporaryReadOrderCollection = new ObservableCollection<SeriesReadOrder>();
            temporaryReadOrderCollection.AddRange(SelectedItem.SeriesReadOrder.OrderBy(a => a.Instalment));
            SelectedItem.SeriesReadOrder.Clear();

            foreach (var item in temporaryReadOrderCollection)
            {
                SelectedItem.SeriesReadOrder.Add(item);
            }
        }

        private bool OnAddBookToSeriesCanExecute(Guid? id)
            => (id is null || id == Guid.Empty) ? false : true;

        private async void OnAddBookToSeriesExecute(Guid? id)
        {
            var addedBook = await (Repository as ISeriesRepository).GetBookById((Guid)id);

            SelectedItem.BooksInSeries.Add(addedBook);

            SelectedItem.SeriesReadOrder.Add(new SeriesReadOrder
            {
                BookId = addedBook.Id,
                Book = addedBook,
                Series = SelectedItem,
                SeriesId = SelectedItem.Id,
                Instalment = SelectedItem.SeriesReadOrder.Count() + 1
            });

            Books.Remove(Books.First(b => b.Id == id));

            SelectedItem.NumberOfBooks = SelectedItem.SeriesReadOrder.Count();

            SetChangeTracker();
        }

        private void OnFilterBookListExecute(string filter)
        {
            if (filter != string.Empty && filter != null)
            {
                var filteredCollection = AllBooks.Where(item => !SelectedItem.BooksInSeries
                                                 .Any(x => x.Id == item.Id))
                                                 .Where(item => item.DisplayMember
                                                    .IndexOf(filter, StringComparison.OrdinalIgnoreCase) != -1)
                                                 .OrderBy(b => b.DisplayMember);

                PopulateBooksCollection(filteredCollection);
            }
            else
            {
                var allExcludingBooksInSeries = AllBooks.Where(item => !SelectedItem.BooksInSeries
                                                        .Any(x => x.Id == item.Id))
                                                        .OrderBy(b => b.DisplayMember);

                PopulateBooksCollection(allExcludingBooksInSeries);
            }
        }

        private void PopulateBooksCollection(IOrderedEnumerable<LookupItem> tempBookCollection)
        {
            Books.Clear();
            foreach (var item in tempBookCollection)
            {
                Books.Add(item);
            }
        }

        private async Task<IEnumerable<LookupItem>> GetBookList()
            => await bookLookupDataService.GetBookLookupAsync(nameof(BookDetailViewModel));

        private async void OnAddSeriesPictureExecute()
        {
            SelectedItem.PicturePath = FileExplorerService.BrowsePicture() ?? SelectedItem.PicturePath;
            await LoadAsync(this.Id);
            SetChangeTracker();
        }

        public void DragOver(IDropInfo dropInfo)
        {
            SeriesReadOrder sourceItem = dropInfo.Data as SeriesReadOrder;
            SeriesReadOrder targetItem = dropInfo.TargetItem as SeriesReadOrder;

            if (sourceItem != null && targetItem != null)
            {
                dropInfo.DropTargetAdorner = DropTargetAdorners.Highlight;
                dropInfo.Effects = DragDropEffects.Copy;
            }
        }

        public void Drop(IDropInfo dropInfo)
        {
            SeriesReadOrder sourceItem = dropInfo.Data as SeriesReadOrder;
            SeriesReadOrder targetItem = dropInfo.TargetItem as SeriesReadOrder;

            var originalSourceInstalment = sourceItem.Instalment;
            var originalTargetInstalment = targetItem.Instalment;


            if (originalTargetInstalment == (originalSourceInstalment + 1)
                || originalTargetInstalment == (originalSourceInstalment - 1))
            {
                originalSourceInstalment = sourceItem.Instalment;

                targetItem.Instalment = originalSourceInstalment;
                sourceItem.Instalment = originalTargetInstalment;

                SelectedItem.SeriesReadOrder.Remove(sourceItem);
                SelectedItem.SeriesReadOrder.Add(sourceItem);
                SelectedItem.SeriesReadOrder.Remove(targetItem);
                SelectedItem.SeriesReadOrder.Add(targetItem);
            }
            else
            {
                sourceItem.Instalment = targetItem.Instalment;
                SelectedItem.SeriesReadOrder.Remove(sourceItem);

                foreach (var item in SelectedItem.SeriesReadOrder)
                {
                    if (item.Instalment > originalSourceInstalment && item.Instalment <= sourceItem.Instalment)
                    {
                        item.Instalment--;
                    }
                    else if (item.Instalment < originalSourceInstalment && item.Instalment >= sourceItem.Instalment)
                    {
                        item.Instalment++;
                    }
                }

                SelectedItem.SeriesReadOrder.Add(sourceItem);

                RefreshSeriesReadOrder();
            }
            SetChangeTracker();
        }
    }
}
