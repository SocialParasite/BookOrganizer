using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using BookOrganizer.Domain;
using BookOrganizer.Domain.Services;
using BookOrganizer.UI.WPFCore.Services;
using BookOrganizer.UI.WPFCore.Wrappers;
using GongSolutions.Wpf.DragDrop;
using Prism.Commands;
using Prism.Events;
using Serilog;

namespace BookOrganizer.UI.WPFCore.ViewModels
{
    public class SeriesDetailViewModel : BaseDetailViewModel<Series, SeriesWrapper>, IDropTarget
    {
        private SeriesWrapper selectedItem;
        private readonly IBookLookupDataService bookLookupDataService;

        public SeriesDetailViewModel(IEventAggregator eventAggregator,
                                     ILogger logger,
                                     IDomainService<Series> domainService,
                                     IBookLookupDataService bookLookupDataService)
            : base(eventAggregator, logger, domainService)
        {
            this.bookLookupDataService = bookLookupDataService ?? throw new ArgumentNullException(nameof(bookLookupDataService));

            AddSeriesPictureCommand = new DelegateCommand(OnAddSeriesPictureExecute);
            FilterBookListCommand = new DelegateCommand<string>(OnFilterBookListExecute);
            AddBookToSeriesCommand = new DelegateCommand<Guid?>(OnAddBookToSeriesExecute, OnAddBookToSeriesCanExecute);
            SaveItemCommand = new DelegateCommand(SaveItemExecute, SaveItemCanExecute)
                .ObservesProperty(() => SelectedItem.Name);
            SelectedItem = new SeriesWrapper(domainService.CreateItem());

            Books = new ObservableCollection<LookupItem>();
            AllBooks = new ObservableCollection<LookupItem>(); 
        }

        public ICommand AddSeriesPictureCommand { get; }
        public ICommand FilterBookListCommand { get; }
        public ICommand AddBookToSeriesCommand { get; }

        public ObservableCollection<LookupItem> Books { get; set; }
        public ObservableCollection<LookupItem> AllBooks { get; set; }

        public override SeriesWrapper SelectedItem
        {
            get { return selectedItem; }
            set
            {
                selectedItem = value ?? throw new ArgumentNullException(nameof(SelectedItem));
                OnPropertyChanged();
            }
        }

        public async override Task LoadAsync(Guid id)
        {
            try
            {
                var series = await domainService.Repository.GetSelectedAsync(id) ?? new Series();

                SelectedItem = CreateWrapper(series);

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
                    if (e.PropertyName == nameof(SelectedItem.Name))
                    {
                        TabTitle = SelectedItem.Name;
                    }
                };
                ((DelegateCommand)SaveItemCommand).RaiseCanExecuteChanged();

                Id = id;

                if (Id != default)
                {
                    TabTitle = SelectedItem.Name;
                }
                else
                {
                    this.SwitchEditableStateExecute();
                    SelectedItem.Name = "";
                }

                await PopulateAllBooksCollection();
                SetDefaultSeriesPictureIfNoneSet();

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
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                logger.Error("Message: {Message}\n\n Stack trace: {StackTrace}\n\n", ex.Message, ex.StackTrace);
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
                    var tempBookCollection = AllBooks.Where(item => !SelectedItem.Model.BooksSeries
                                                     .Any(x => x.SeriesId == SelectedItem.Id && x.BookId == item.Id))
                                                     .OrderBy(b => b.DisplayMember);

                    PopulateBooksCollection(tempBookCollection);
                }
            }
        }

        public override void OnShowSelectedBookExecute(Guid? id)
        {
            if (UserMode.Item2 == DetailViewState.ViewMode)
            {
                base.OnShowSelectedBookExecute(id);
            }
            else
            {
                var booksSeries = SelectedItem.Model.BooksSeries
                                                .Where(s => s.SeriesId == SelectedItem.Id && s.BookId == id)
                                                .First();
                SelectedItem.Model.BooksSeries.Remove(booksSeries);

                var sro = SelectedItem.Model.SeriesReadOrder.First(b => b.BookId == id);
                SelectedItem.Model.SeriesReadOrder.Remove(sro);

                Books.Add(new LookupItem { Id = booksSeries.BookId, DisplayMember = booksSeries.Book.Title, Picture = booksSeries.Book.BookCoverPicturePath });

                var countOfBooksInSeries = SelectedItem.Model.SeriesReadOrder.Count();
                SelectedItem.NumberOfBooks = countOfBooksInSeries;

                var inst = sro.Instalment;
                if (inst < ++countOfBooksInSeries)
                {
                    foreach (var item in SelectedItem.Model.SeriesReadOrder)
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
            temporaryReadOrderCollection.AddRange(SelectedItem.Model.SeriesReadOrder.OrderBy(a => a.Instalment));
            SelectedItem.Model.SeriesReadOrder.Clear();

            foreach (var item in temporaryReadOrderCollection)
            {
                SelectedItem.Model.SeriesReadOrder.Add(item);
            }
        }

        private bool OnAddBookToSeriesCanExecute(Guid? id)
            => (id is null || id == Guid.Empty) ? false : true;

        private async void OnAddBookToSeriesExecute(Guid? id)
        {
            var addedBook = await (domainService.Repository as ISeriesRepository).GetBookById((Guid)id);

            var booksSeries = new BooksSeries
            {
                Book = addedBook,
                BookId = addedBook.Id,
                Series = SelectedItem.Model,
                SeriesId = SelectedItem.Id
            };
            SelectedItem.Model.BooksSeries.Add(booksSeries);

            SelectedItem.Model.SeriesReadOrder
                .Add(new SeriesReadOrder
                {
                    BookId = addedBook.Id,
                    Book = addedBook,
                    Series = SelectedItem.Model,
                    SeriesId = SelectedItem.Id,
                    Instalment = SelectedItem.Model.SeriesReadOrder.Count + 1
                });

            Books.Remove(Books.First(b => b.Id == id));

            SelectedItem.NumberOfBooks = SelectedItem.Model.SeriesReadOrder.Count();

            SetChangeTracker();
        }

        private void OnFilterBookListExecute(string filter)
        {
            if (filter != string.Empty && filter != null)
            {
                var filteredCollection = AllBooks.Where(item => !SelectedItem.Model.BooksSeries
                                                 .Any(x => x.SeriesId == SelectedItem.Id && x.BookId == item.Id))
                                                 .Where(item => item.DisplayMember
                                                    .IndexOf(filter, StringComparison.OrdinalIgnoreCase) != -1)
                                                 .OrderBy(b => b.DisplayMember);

                PopulateBooksCollection(filteredCollection);
            }
            else
            {
                var allExcludingBooksInSeries = AllBooks.Where(item => !SelectedItem.Model.BooksSeries
                                                        .Any(x => x.SeriesId == SelectedItem.Id && x.BookId == item.Id))
                                                        .OrderBy(b => b.DisplayMember);

                PopulateBooksCollection(allExcludingBooksInSeries);
            }
        }

        private void PopulateBooksCollection(IOrderedEnumerable<LookupItem> tempBookCollection)
        {
            if (Books.Count == 0 && !tempBookCollection.Any())
            {
                Books = AllBooks;
            }
            else
            {
                Books.Clear();
                foreach (var item in tempBookCollection)
                {
                    Books.Add(item);
                } 
            }
        }

        private async Task<IEnumerable<LookupItem>> GetBookList()
            => await bookLookupDataService.GetBookLookupAsync(nameof(BookDetailViewModel));

        private void OnAddSeriesPictureExecute()
        {
            SelectedItem.PicturePath = FileExplorerService.BrowsePicture() ?? SelectedItem.PicturePath;
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

                SelectedItem.Model.SeriesReadOrder.Remove(sourceItem);
                SelectedItem.Model.SeriesReadOrder.Add(sourceItem);
                SelectedItem.Model.SeriesReadOrder.Remove(targetItem);
                SelectedItem.Model.SeriesReadOrder.Add(targetItem);
            }
            else
            {
                sourceItem.Instalment = targetItem.Instalment;
                SelectedItem.Model.SeriesReadOrder.Remove(sourceItem);

                foreach (var item in SelectedItem.Model.SeriesReadOrder)
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

                SelectedItem.Model.SeriesReadOrder.Add(sourceItem);

                RefreshSeriesReadOrder();
            }
            SetChangeTracker();
        }

        public override SeriesWrapper CreateWrapper(Series entity)
        {
            var wrapper = new SeriesWrapper(entity);
            return wrapper;
        }
    }
}

