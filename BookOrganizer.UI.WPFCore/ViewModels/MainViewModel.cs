using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Autofac.Features.Indexed;
using BookOrganizer.UI.WPFCore.Events;
using Prism.Commands;
using Prism.Events;
using Serilog;

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
        private char pinGlyph;
        private readonly ILogger logger;

        public MainViewModel(IEventAggregator eventAggregator,
                              IIndex<string, IDetailViewModel> detailViewModelCreator,
                              IIndex<string, ISelectedViewModel> viewModelCreator,
                              ILogger logger)
        {
            this.detailViewModelCreator = detailViewModelCreator ?? throw new ArgumentNullException(nameof(detailViewModelCreator));
            this.viewModelCreator = viewModelCreator ?? throw new ArgumentNullException(nameof(viewModelCreator));
            this.eventAggregator = eventAggregator ?? throw new ArgumentNullException(nameof(eventAggregator));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));

            DetailViewModels = new ObservableCollection<IDetailViewModel>();
            TEMP_DetailViewModels = new ObservableCollection<IDetailViewModel>();

            OpenSelectedViewCommand = new DelegateCommand<string>(OnOpenSelectedViewExecute);
            
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

                this.eventAggregator.GetEvent<OpenItemMatchingSelectedAuthorIdEvent<Guid>>()
                        .Subscribe(OnOpenAuthorMatchingSelectedId);

                //this.eventAggregator.GetEvent<OpenItemMatchingSelectedSeriesIdEvent<Guid>>()
                //    .Subscribe(OnOpenSeriesMatchingSelectedId);

                this.eventAggregator.GetEvent<OpenDetailViewEvent>()
                    .Subscribe(OnOpenDetailViewMatchingSelectedId);

                this.eventAggregator.GetEvent<CloseDetailsViewEvent>()
                    .Subscribe(CloseDetailsView);

                this.eventAggregator.GetEvent<SavedDetailsViewEvent>()
                    .Subscribe(OnSaveDetailsView);
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
                isMenuBarVisible = SwitchMenuVisibility(value);
                OnPropertyChanged();
            }
        }

        private bool SwitchMenuVisibility(bool value)
        {
            PinGlyph = value ? '\uE718' : '\uE77A';

            return value;
        }

        public char PinGlyph
        {
            get { return pinGlyph; }
            set
            {
                pinGlyph = value;
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
                    logger.Error("Message: {Message}\n\n Stack trace: {StackTrace}\n\n", ex.Message, ex.StackTrace);
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

            if (DetailViewModels.Count == 0)
            {
                IsViewVisible = true;
            }
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
            if (SelectedVM is IItemLists)
            {
                (SelectedVM as IItemLists).InitializeRepositoryAsync();
            }
            //RemoveDetailViewModel(default, args.ViewModelName);

            //OnOpenDetailViewMatchingSelectedId(args);
        }

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
