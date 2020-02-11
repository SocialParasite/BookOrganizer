using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Autofac.Features.Indexed;
using BookOrganizer.UI.WPFCore.Events;
using Prism.Commands;
using Prism.Events;

namespace BookOrganizer.UI.WPFCore.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly IEventAggregator eventAggregator;
        private IDetailViewModel _selectedDetailViewModel;
        private IIndex<string, IDetailViewModel> detailViewModelCreator;
        private IIndex<string, ISelectedViewModel> viewModelCreator;
        private ISelectedViewModel selectedVM;
        private bool isViewVisible;
        private bool isMenuBarVisible;

        public MainViewModel(IEventAggregator eventAggregator,
                              IIndex<string, IDetailViewModel> detailViewModelCreator,
                              IIndex<string, ISelectedViewModel> viewModelCreator)
        {
            this.detailViewModelCreator = detailViewModelCreator ?? throw new ArgumentNullException(nameof(detailViewModelCreator));
            this.viewModelCreator = viewModelCreator ?? throw new ArgumentNullException(nameof(viewModelCreator));
            this.eventAggregator = eventAggregator ?? throw new ArgumentNullException(nameof(eventAggregator));

            DetailViewModels = new ObservableCollection<IDetailViewModel>();
            TEMP_DetailViewModels = new ObservableCollection<IDetailViewModel>();

            OpenSelectedViewCommand = new DelegateCommand<string>(OnOpenSelectedViewExecute);
            //ShowMenuCommand = new DelegateCommand(OnShowMenuExecute);
            CreateNewItemCommand = new DelegateCommand<Type>(OnCreateNewItemExecute);

            IsMenuBarVisible = true;

            OnOpenSelectedViewExecute(nameof(MainPageViewModel));

            SubscribeToEvents();
        }


        private void SubscribeToEvents()
        {
            if (eventAggregator.GetEvent<OpenItemMatchingSelectedBookIdEvent<Guid>>() != null)
            {
                this.eventAggregator.GetEvent<OpenItemViewEvent>()
                    .Subscribe(OnOpenSelectedItemView);

                //this.eventAggregator.GetEvent<OpenItemMatchingSelectedBookIdEvent<Guid>>()
                //        .Subscribe(OnOpenBookMatchingSelectedId);

                //this.eventAggregator.GetEvent<OpenItemMatchingSelectedPublisherIdEvent<Guid>>()
                //        .Subscribe(OnOpenPublisherMatchingSelectedId);

                //this.eventAggregator.GetEvent<OpenItemMatchingSelectedAuthorIdEvent<Guid>>()
                //        .Subscribe(OnOpenAuthorMatchingSelectedId);

                //this.eventAggregator.GetEvent<OpenItemMatchingSelectedSeriesIdEvent<Guid>>()
                //    .Subscribe(OnOpenSeriesMatchingSelectedId);

                //this.eventAggregator.GetEvent<OpenDetailViewEvent>()
                //    .Subscribe(OnOpenDetailViewMatchingSelectedId);

                //this.eventAggregator.GetEvent<CloseDetailsViewEvent>()
                //    .Subscribe(CloseDetailsView);

                //this.eventAggregator.GetEvent<SavedDetailsViewEvent>()
                //    .Subscribe(OnSaveDetailsView);
            }
        }

        public ICommand ShowMenuCommand { get; }
        public ICommand OpenSelectedViewCommand { get; }
        public ICommand CreateNewItemCommand { get; set; }

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

        public bool IsViewVisible
        {
            get => isViewVisible;
            set
            {
                isViewVisible = value;
                OnPropertyChanged();
            }
        }

        public bool IsMenuBarVisible 
        {
            get => isMenuBarVisible; 
            set
            {
                isMenuBarVisible = value;
                OnPropertyChanged();
            }
        }

        public ISelectedViewModel SelectedVM
        {
            get { return selectedVM; }
            set
            {
                selectedVM = value;
                OnPropertyChanged();
            }
        }

        private void OnOpenSelectedItemView(OpenItemViewEventArgs args)
        {
            OnOpenSelectedViewExecute(args.ViewModelName);
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
                    MessageBox.Show(ex.Message);
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
            RemoveDetailViewModel(default, args.ViewModelName);

            OnOpenDetailViewMatchingSelectedId(args);
        }
        //private void OnShowMenuExecute()
        //{
        //    IsMenuBarVisible = !IsMenuBarVisible;
        //}

        private void OnCreateNewItemExecute(Type itemType)
        {
            if (itemType != null)
            {
                eventAggregator.GetEvent<OpenDetailViewEvent>()
                               .Publish(new OpenDetailViewEventArgs
                               {
                                   Id = new Guid(),
                                   ViewModelName = Type.GetType(itemType.FullName).Name
                               });
            }

        }
    }
}
