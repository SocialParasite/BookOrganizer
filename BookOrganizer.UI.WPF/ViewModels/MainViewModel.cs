using Autofac.Features.Indexed;
using BookOrganizer.UI.WPF.Events;
using BookOrganizer.UI.WPF.Lookups;
using Prism.Commands;
using Prism.Events;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
        private bool isMenuBarVisible;

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
            TEMP_DetailViewModels = new ObservableCollection<IDetailViewModel>();

            OpenMainMenuCommand = new DelegateCommand(OnOpenMainMenuExecute);
            OpenSettingsMenuCommand = new DelegateCommand(OnOpenSettingsMenuExecute);
            OpenSelectedViewCommand = new DelegateCommand<string>(OnOpenSelectedViewExecute);
            ShowMenuCommand = new DelegateCommand(OnShowMenuExecute);

            IsMenuBarVisible = false;

            SubscribeToEvents();
        }

        private void SubscribeToEvents()
        {
            if (eventAggregator.GetEvent<OpenItemMatchingSelectedBookIdEvent<Guid>>() != null)
            {
                this.eventAggregator.GetEvent<OpenItemMatchingSelectedBookIdEvent<Guid>>()
                        .Subscribe(OnOpenBookMatchingSelectedId);

                this.eventAggregator.GetEvent<OpenItemMatchingSelectedPublisherIdEvent<Guid>>()
                        .Subscribe(OnOpenPublisherMatchingSelectedId);

                this.eventAggregator.GetEvent<OpenItemMatchingSelectedAuthorIdEvent<Guid>>()
                        .Subscribe(OnOpenAuthorMatchingSelectedId);

                this.eventAggregator.GetEvent<OpenItemMatchingSelectedSeriesIdEvent<Guid>>()
                    .Subscribe(OnOpenSeriesMatchingSelectedId);

                this.eventAggregator.GetEvent<OpenDetailViewEvent>()
                    .Subscribe(OnOpenDetailViewMatchingSelectedId);

                this.eventAggregator.GetEvent<CloseDetailsViewEvent>()
                    .Subscribe(CloseDetailsView);

                this.eventAggregator.GetEvent<SavedDetailsViewEvent>()
                    .Subscribe(OnSaveDetailsView);
            }
        }

        public ICommand ShowMenuCommand { get; }
        public ICommand OpenMainMenuCommand { get; }
        public ICommand OpenSettingsMenuCommand { get; }
        public ICommand OpenSelectedViewCommand { get; }

        public ObservableCollection<IDetailViewModel> DetailViewModels { get; }
        public ObservableCollection<IDetailViewModel> TEMP_DetailViewModels { get; }

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

        public bool IsMenuBarVisible { get => isMenuBarVisible; set { isMenuBarVisible = value; OnPropertyChanged(); } }

        public ISelectedViewModel SelectedVM
        {
            get { return selectedVM; }
            set
            {
                selectedVM = value;
                OnPropertyChanged();
            }
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

        private void OnOpenSelectedViewExecute(string viewModel)
        {
            SelectedVM = viewModelCreator[viewModel];
            IsViewVisible = true;
        }

        [Obsolete]
        private void OnOpenMainMenuExecute()
        {
            throw new NotImplementedException();
        }

        [Obsolete]
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

        private void OnOpenPublisherMatchingSelectedId(Guid publisherId)
        {
            OnOpenDetailViewMatchingSelectedId(
                new OpenDetailViewEventArgs
                {
                    Id = publisherId,
                    ViewModelName = nameof(PublisherDetailViewModel)
                });
        }

        private void OnOpenAuthorMatchingSelectedId(Guid authorId)
        {
            OnOpenDetailViewMatchingSelectedId(
                new OpenDetailViewEventArgs
                {
                    Id = authorId,
                    ViewModelName = nameof(AuthorDetailViewModel)
                });
        }

        private void OnOpenSeriesMatchingSelectedId(Guid seriesId)
        {
            OnOpenDetailViewMatchingSelectedId(
                new OpenDetailViewEventArgs
                {
                    Id = seriesId,
                    ViewModelName = nameof(SeriesDetailViewModel)
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
                && vm.GetType().Name == viewModelName);

            if (detailViewModel != null)
            {
                DetailViewModels.Remove(detailViewModel);
            }
        }

        private void OnSaveDetailsView(OpenDetailViewEventArgs args)
        {
            RemoveDetailViewModel(Guid.Parse("00000000-0000-0000-0000-000000000000"), args.ViewModelName);

            OnOpenDetailViewMatchingSelectedId(args);
        }
        private void OnShowMenuExecute()
        {
            IsMenuBarVisible = !IsMenuBarVisible;
        }



    }
}
