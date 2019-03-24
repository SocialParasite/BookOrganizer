﻿using Autofac.Features.Indexed;
using BookOrganizer.UI.WPF.Events;
using BookOrganizer.UI.WPF.Lookups;
using Prism.Commands;
using Prism.Events;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace BookOrganizer.UI.WPF.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly IEventAggregator eventAggregator;
        private IDetailViewModel _selectedDetailViewModel;
        private IIndex<string, IDetailViewModel> detailViewModelCreator;
        private IIndex<string, ISelectedViewModel> viewModelCreator;
        private readonly IBookLookupDataService bookLookupDataService;

        private ISelectedViewModel selectedVM;
        private bool isViewVisible;

        public MainViewModel(IEventAggregator eventAggregator,
                              IIndex<string, IDetailViewModel> detailViewModelCreator,
                              IIndex<string, ISelectedViewModel> viewModelCreator,
                              IBookLookupDataService bookLookupDataService)
        {
            this.detailViewModelCreator = detailViewModelCreator ?? throw new ArgumentNullException(nameof(detailViewModelCreator));
            this.viewModelCreator = viewModelCreator ?? throw new ArgumentNullException(nameof(viewModelCreator));
            this.bookLookupDataService = bookLookupDataService ?? throw new ArgumentNullException(nameof(bookLookupDataService));
            this.eventAggregator = eventAggregator ?? throw new ArgumentNullException(nameof(eventAggregator));

            DetailViewModels = new ObservableCollection<IDetailViewModel>();

            OpenMainMenuCommand = new DelegateCommand(OnOpenMainMenuExecute);
            OpenBooksViewCommand = new DelegateCommand<string>(OnOpenBooksViewExecute);
            OpenAuthorsViewCommand = new DelegateCommand(OnOpenAuthorsViewExecute);
            OpenPublishersViewCommand = new DelegateCommand<string>(OnOpenPublishersViewExecute);
            OpenSettingsMenuCommand = new DelegateCommand(OnOpenSettingsMenuExecute);

            SubscribeToEvents();
        }

        private void SubscribeToEvents()
        {
            if (eventAggregator.GetEvent<OpenItemMatchingSelectedBookIdEvent<Guid>>() != null)
            {
                this.eventAggregator.GetEvent<OpenItemMatchingSelectedBookIdEvent<Guid>>()
                        .Subscribe(OnOpenBookMatchingSelectedId);

                this.eventAggregator.GetEvent<OpenDetailViewEvent>()
                    .Subscribe(OnOpenDetailViewMatchingSelectedId);

                this.eventAggregator.GetEvent<CloseDetailsViewEvent>()
                    .Subscribe(CloseDetailsView);
            }
        }

        public ICommand OpenMainMenuCommand { get; }
        public ICommand OpenBooksViewCommand { get; }
        public ICommand OpenAuthorsViewCommand { get; }
        public ICommand OpenPublishersViewCommand { get; }
        public ICommand OpenSettingsMenuCommand { get; }

        public ObservableCollection<IDetailViewModel> DetailViewModels { get; set; }

        public IDetailViewModel SelectedDetailViewModel
        {
            get { return _selectedDetailViewModel; }
            set
            {
                _selectedDetailViewModel = value;
                OnPropertyChanged();
            }
        }

        public bool IsViewVisible { get => isViewVisible; set { isViewVisible = value; OnPropertyChanged(); } }

        public ISelectedViewModel SelectedVM
        {
            get { return selectedVM; }
            set { selectedVM = value; OnPropertyChanged(); }
        }

        private async void OnOpenDetailViewMatchingSelectedId(OpenDetailViewEventArgs args)
        {
            var detailViewModel = DetailViewModels
                .SingleOrDefault(vm => vm.Id == args.Id
                && vm.GetType().Name == args.ViewModelName);

            if (detailViewModel is null)
            {
                detailViewModel = detailViewModelCreator[args.ViewModelName];
                try
                {
                    await detailViewModel.LoadAsync(args.Id);
                }
                catch (Exception ex)
                {
                    //TODO
                    return;
                }

                DetailViewModels.Add(detailViewModel);
                SelectedDetailViewModel = DetailViewModels.Last();
            }
            else
                SelectedDetailViewModel = DetailViewModels.SingleOrDefault(b => b.Id == args.Id);

            IsViewVisible = false;
        }

        private void OnOpenPublishersViewExecute(string viewModel)
        {
            if (!IsViewVisible)
            {
                SelectedVM = viewModelCreator[viewModel];
                IsViewVisible = true;
            }
        }

        private void OnOpenAuthorsViewExecute()
        {
            throw new NotImplementedException();
        }

        private void OnOpenBooksViewExecute(string viewModel)
        {
            if (!IsViewVisible)
            {
                SelectedVM = viewModelCreator[viewModel];
                IsViewVisible = true;
            }
            //else
            //{
            //    SelectedVM = null;
            //    IsViewVisible = false;
            //}
        }

        private void OnOpenMainMenuExecute()
        {
            throw new NotImplementedException();
        }

        private void OnOpenSettingsMenuExecute()
        {
            throw new NotImplementedException();
        }

        private void OnOpenBookMatchingSelectedId(Guid bookId)
        {
            OnOpenDetailViewMatchingSelectedId(
               new OpenDetailViewEventArgs
               {
                   Id = bookId,
                   ViewModelName = nameof(BookDetailViewModel)
               });
        }

        private void CloseDetailsView(CloseDetailsViewEventArgs args)
        {
            RemoveDetailViewModel(args.Id, args.ViewModelName);
        }

        private void RemoveDetailViewModel(Guid id, string viewModelName)
        {
            var detailViewModel = DetailViewModels
                .SingleOrDefault(vm => vm.Id == id
                /*&& vm.GetType().Name == viewModelName*/);

            if (detailViewModel != null)
            {
                DetailViewModels.Remove(detailViewModel);
            }
        }
    }
}
