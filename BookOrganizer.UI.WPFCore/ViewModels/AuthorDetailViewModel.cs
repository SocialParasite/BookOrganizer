using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
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
    public class AuthorDetailViewModel : BaseDetailViewModel<Author, AuthorWrapper>
    {
        private LookupItem selectedNationality;
        private AuthorWrapper selectedItem;

        public AuthorDetailViewModel(IEventAggregator eventAggregator,
                                     ILogger logger,
                                     IDomainService<Author> domainService)
            : base(eventAggregator, logger, domainService)
        {
            AddAuthorPictureCommand = new DelegateCommand(OnAddAuthorPictureExecute);
            AddNewNationalityCommand = new DelegateCommand(OnAddNewNationalityExecute);
            NationalitySelectionChangedCommand = new DelegateCommand(OnNationalitySelectionChangedExecute);

            SelectedItem = new AuthorWrapper(domainService.CreateItem());

            Nationalities = new ObservableCollection<LookupItem>();
        }

        public ICommand AddAuthorPictureCommand { get; }
        public ICommand NationalitySelectionChangedCommand { get; }
        public ICommand AddNewNationalityCommand { get; }

        public LookupItem SelectedNationality
        {
            get { return selectedNationality; }
            set { selectedNationality = value; OnPropertyChanged(); }
        }

        public ObservableCollection<LookupItem> Nationalities { get; set; }

        public override AuthorWrapper SelectedItem
        {
            get { return selectedItem; }
            set
            {
                selectedItem = value ?? throw new ArgumentNullException(nameof(SelectedItem));
                OnPropertyChanged();
            }
        }

        private void SetTabTitle()
            => TabTitle = $"{SelectedItem.Model.LastName}, {SelectedItem.Model.FirstName}";

        private void OnAddAuthorPictureExecute()
        {
            SelectedItem.MugShotPath = FileExplorerService.BrowsePicture() ?? SelectedItem.MugShotPath;
        }

        public async override Task LoadAsync(Guid id)
        {
            try
            {
                var author = await domainService.Repository.GetSelectedAsync(id) ?? new Author();

                SelectedItem = CreateWrapper(author);

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
                    if (e.PropertyName == nameof(SelectedItem.FirstName)
                        || e.PropertyName == nameof(SelectedItem.LastName))
                    {
                        SetTabTitle();
                    }
                };
                ((DelegateCommand)SaveItemCommand).RaiseCanExecuteChanged();

                Id = id;

                if (Id != default)
                {
                    SetTabTitle();
                }
                else
                {
                    this.SwitchEditableStateExecute();
                    SelectedItem.FirstName = "";
                    SelectedItem.LastName = "";
                }

                SetDefaultAuthorPicIfNoneSet();

                InitiliazeSelectedNationalityIfNoneSet();

                void SetDefaultAuthorPicIfNoneSet()
                {
                    if (SelectedItem.MugShotPath is null)
                        SelectedItem.MugShotPath = FileExplorerService.GetImagePath();
                }

                void InitiliazeSelectedNationalityIfNoneSet()
                {
                    if (SelectedNationality is null && SelectedItem.Model.Nationality != null)
                    {
                        SelectedNationality =
                            new LookupItem
                            {
                                Id = SelectedItem.Model.Nationality.Id,
                                DisplayMember = SelectedItem.Model.Nationality is null
                                ? new Nationality().Name = ""
                                : SelectedItem.Model.Nationality.Name
                            };
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

            await InitializeNationalityCollection();

            async Task InitializeNationalityCollection()
            {
                if (!Nationalities.Any())
                {
                    Nationalities.Clear();

                    foreach (var item in await GetNationalityList())
                    {
                        Nationalities.Add(item);
                    }

                    if (SelectedItem.Model.Nationality != null)
                        SelectedNationality = Nationalities.FirstOrDefault(n => n.Id == SelectedItem.Model.Nationality.Id);
                }
            }
        }

        private async Task<IEnumerable<LookupItem>> GetNationalityList()
            => await (domainService as AuthorService).NationalityLookupDataService
                                                     .GetNationalityLookupAsync(nameof(NationalityDetailViewModel));

        private void OnNationalitySelectionChangedExecute()
        {
            if (SelectedNationality != null && Nationalities.Any())
            {
                SelectedItem.Model.NationalityId = SelectedNationality.Id;
                SetChangeTracker();
            }
        }


        private void OnAddNewNationalityExecute()
        {
            eventAggregator.GetEvent<OpenDetailViewEvent>()
                           .Publish(new OpenDetailViewEventArgs
                           {
                               Id = new Guid(),
                               ViewModelName = nameof(NationalityDetailViewModel)
                           });
        }

        public override AuthorWrapper CreateWrapper(Author entity)
        {
            var wrapper = new AuthorWrapper(entity);
            return wrapper;
        }
    }
}
