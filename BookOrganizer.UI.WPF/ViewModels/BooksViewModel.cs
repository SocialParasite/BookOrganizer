﻿using BookOrganizer.Domain;
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
        {
            this.bookLookupDataService = bookLookupDataService ?? throw new ArgumentNullException(nameof(bookLookupDataService));
            this.eventAggregator = eventAggregator ?? throw new ArgumentNullException(nameof(eventAggregator));

            BookTitleLabelMouseLeftButtonUpCommand
                = new DelegateCommand<Guid?>(OnBookTitleLabelMouseLeftButtonUpExecute, OnBookTitleLabelMouseLeftButtonUpCanExecute);


            InitializeRepositoryAsync();
        }

        readonly IEventAggregator eventAggregator;

        public ICommand BookTitleLabelMouseLeftButtonUpCommand { get; }

        private OpenDetailViewEventArgs selectedBook;
        public OpenDetailViewEventArgs SelectedBook
        {
            get => selectedBook;
            set
            {
                selectedBook = value;
                OnPropertyChanged();
                if (selectedBook != null)
                {
                    eventAggregator.GetEvent<OpenDetailViewEvent>()
                                   .Publish(selectedBook);
                }
            }
        }

        public override async Task InitializeRepositoryAsync()
        {
            Items = await bookLookupDataService.GetBookLookupAsync();

            EntityCollection = Items.OrderBy(b => b.DisplayMember).ToList();
        }

        private void OnBookTitleLabelMouseLeftButtonUpExecute(Guid? id)
            => OnOpenBookMatchingSelectedId((Guid)id);

        private bool OnBookTitleLabelMouseLeftButtonUpCanExecute(Guid? id)
            => (id is null || id == Guid.Empty) ? false : true;

        private void OnOpenBookMatchingSelectedId(Guid id)
            => SelectedBook = new OpenDetailViewEventArgs { Id = id, ViewModelName = nameof(BookDetailViewModel) };

        //public ISelectedViewModel CreateView()
        //{
        //    throw new NotImplementedException();
        //}
    }
}