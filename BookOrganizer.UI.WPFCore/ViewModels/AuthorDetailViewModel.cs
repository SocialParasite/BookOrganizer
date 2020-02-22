using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using BookOrganizer.Domain;
using BookOrganizer.Domain.Services;
using BookOrganizer.UI.WPFCore.Services;
using BookOrganizer.UI.WPFCore.Wrappers;
using Prism.Commands;
using Prism.Events;

namespace BookOrganizer.UI.WPFCore.ViewModels
{
    public class AuthorDetailViewModel : BaseDetailViewModel<Author, AuthorWrapper> 
    {
        private LookupItem selectedNationality;
        private readonly INationalityLookupDataService nationalityLookupDataService;
        private AuthorWrapper selectedItem;

        public AuthorDetailViewModel(IEventAggregator eventAggregator,
            IRepository<Author> repository,
            INationalityLookupDataService nationalityLookupDataService)
            : base(eventAggregator, repository)
        {
            this.nationalityLookupDataService = nationalityLookupDataService
                ?? throw new ArgumentNullException(nameof(nationalityLookupDataService));

            AddAuthorPictureCommand = new DelegateCommand(OnAddAuthorPictureExecute);
            NationalitySelectionChangedCommand = new DelegateCommand(OnNationalitySelectionChangedExecute);

            Nationalities = new ObservableCollection<LookupItem>();
        }

        public ICommand AddAuthorPictureCommand { get; }
        public ICommand NationalitySelectionChangedCommand { get; }

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

        private async void OnAddAuthorPictureExecute()
        {
            SelectedItem.MugShotPath = FileExplorerService.BrowsePicture() ?? SelectedItem.MugShotPath;

            await LoadAsync(SelectedItem.Id);
        }

        public async override Task LoadAsync(Guid id)
        {
            var author = await Repository.GetSelectedAsync(id) ?? new Author();
            SelectedItem = CreateWrapper(author);
            
            SelectedItem.PropertyChanged += (s, e) =>
            {
                if (!HasChanges)
                {
                    HasChanges = Repository.HasChanges();
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
                //SelectedItem.Model.LastName = SelectedItem.Model.LastName; //??
                //SelectedItem.Model.FirstName = SelectedItem.Model.FirstName; //??
                SetTabTitle();
            }
            else
                this.SwitchEditableStateExecute();

            SetDefaultAuthorPicIfNoneSet();

            InitiliazeSelectedNationalityIfNoneSet();

            //SelectedItem.Model.PropertyChanged += (s, e) =>
            //{
            //    SetChangeTracker();
            //};

            void SetDefaultAuthorPicIfNoneSet()
            {
                //if (SelectedItem.MugShotPath is null)
                //    SelectedItem.MugShotPath = FileExplorerService.GetImagePath();
            }

            void InitiliazeSelectedNationalityIfNoneSet()
            {
                if (SelectedNationality is null)
                {
                    if (SelectedItem.Model.Nationality != null)
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
            => await nationalityLookupDataService.GetNationalityLookupAsync(nameof(NationalityDetailViewModel));

        private void OnNationalitySelectionChangedExecute()
        {
            if (SelectedNationality != null && Nationalities.Any())
            {
                SelectedItem.Model.NationalityId = SelectedNationality.Id;
                SetChangeTracker();
            }
        }

        public override AuthorWrapper CreateWrapper(Author entity)
        {
            var wrapper = new AuthorWrapper(entity);
            return wrapper;
        }
    }
}
