using BookOrganizer.DA;
using BookOrganizer.Domain;
using BookOrganizer.UI.WPFCore.Wrappers;
using Prism.Commands;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BookOrganizer.UI.WPFCore.ViewModels
{
    public class AuthorDetailViewModel //: BaseDetailViewModel<Author>
    {
        //private string firstName;
        //private string lastName;
        //private LookupItem selectedNationality;
        //private readonly INationalityLookupDataService nationalityLookupDataService;
        //private AuthorWrapper authorWrapper;

        //public AuthorDetailViewModel(IEventAggregator eventAggregator,
        //    IRepository<Author> authorRepo,
        //    INationalityLookupDataService nationalityLookupDataService, AuthorWrapper authorWrapper)
        //    : base(eventAggregator)
        //{
        //    Repository = authorRepo ?? throw new ArgumentNullException(nameof(authorRepo));
        //    this.nationalityLookupDataService = nationalityLookupDataService
        //        ?? throw new ArgumentNullException(nameof(nationalityLookupDataService));
        //    this.authorWrapper = authorWrapper ?? throw new ArgumentNullException(nameof(authorWrapper));

        //    AddAuthorPictureCommand = new DelegateCommand(OnAddAuthorPictureExecute);
        //    NationalitySelectionChangedCommand = new DelegateCommand(OnNationalitySelectionChangedExecute);

        //    SelectedItem = new Author();
        //    Nationalities = new ObservableCollection<LookupItem>();
        //}

        //public ICommand AddAuthorPictureCommand { get; }
        //public ICommand NationalitySelectionChangedCommand { get; }

        ////[MinLength(1, ErrorMessage = "Authors first name should be at minimum 1 character long.")]
        ////[MaxLength(50, ErrorMessage = "Authors first name should be at maximum 50 character long.")]
        ////[Required]
        ////public string FirstName
        ////{
        ////    get => firstName;
        ////    set
        ////    {
        ////        ValidatePropertyInternal(nameof(FirstName), value);
        ////        firstName = value;
        ////        OnPropertyChanged();
        ////        SetTabTitle();
        ////        ((DelegateCommand)SaveItemCommand).RaiseCanExecuteChanged();
        ////        SelectedItem.FirstName = value;
        ////    }
        ////}

        ////[MinLength(1, ErrorMessage = "Authors last name should be at minimum 1 character long.")]
        ////[MaxLength(50, ErrorMessage = "Authors last name should be at maximum 50 character long.")]
        ////[Required]
        ////public string LastName
        ////{
        ////    get => lastName;
        ////    set
        ////    {
        ////        ValidatePropertyInternal(nameof(LastName), value);
        ////        lastName = value;
        ////        OnPropertyChanged();
        ////        SetTabTitle();
        ////        ((DelegateCommand)SaveItemCommand).RaiseCanExecuteChanged();
        ////        SelectedItem.LastName = value;
        ////    }
        ////}

        //public LookupItem SelectedNationality
        //{
        //    get { return selectedNationality; }
        //    set { selectedNationality = value; OnPropertyChanged(); }
        //}

        //public ObservableCollection<LookupItem> Nationalities { get; set; }

        //private void SetTabTitle()
        //    => TabTitle = $"{LastName}, {FirstName}";

        //private async void OnAddAuthorPictureExecute()
        //{
        //    //SelectedItem.MugShotPath = FileExplorerService.BrowsePicture() ?? SelectedItem.MugShotPath;

        //    await LoadAsync(this.Id);
        //}

        //public async override Task LoadAsync(Guid id)
        //{
        //    SelectedItem = await Repository.GetSelectedAsync(id) ?? new Author();
        //    authorWrapper = new AuthorWrapper(SelectedItem);

        //    Id = id;

        //    if (Id != default)
        //    {
        //        LastName = SelectedItem.LastName;
        //        FirstName = SelectedItem.FirstName;
        //        SetTabTitle();
        //    }
        //    else
        //        this.SwitchEditableStateExecute();

        //    SetDefaultAuthorPicIfNoneSet();

        //    InitiliazeSelectedNationalityIfNoneSet();

        //    SelectedItem.PropertyChanged += (s, e) =>
        //    {
        //        SetChangeTracker();
        //    };

        //    void SetDefaultAuthorPicIfNoneSet()
        //    {
        //        //if (SelectedItem.MugShotPath is null)
        //        //    SelectedItem.MugShotPath = FileExplorerService.GetImagePath();
        //    }

        //    void InitiliazeSelectedNationalityIfNoneSet()
        //    {
        //        if (SelectedNationality is null)
        //        {
        //            if (SelectedItem.Nationality != null)
        //            {
        //                SelectedNationality =
        //                    new LookupItem
        //                    {
        //                        Id = SelectedItem.Nationality.Id,
        //                        DisplayMember = SelectedItem.Nationality is null
        //                        ? new Nationality().Name = ""
        //                        : SelectedItem.Nationality.Name
        //                    };
        //            }
        //        }
        //    }
        //}

        //public override async void SwitchEditableStateExecute()
        //{
        //    base.SwitchEditableStateExecute();

        //    await InitializeNationalityCollection();

        //    async Task InitializeNationalityCollection()
        //    {
        //        if (!Nationalities.Any())
        //        {
        //            Nationalities.Clear();

        //            foreach (var item in await GetNationalityList())
        //            {
        //                Nationalities.Add(item);
        //            }

        //            if (SelectedItem.Nationality != null)
        //                SelectedNationality = Nationalities.FirstOrDefault(n => n.Id == SelectedItem.Nationality.Id);
        //        }
        //    }
        //}

        //private async Task<IEnumerable<LookupItem>> GetNationalityList()
        //    => await nationalityLookupDataService.GetNationalityLookupAsync(nameof(NationalityDetailViewModel));

        //private void OnNationalitySelectionChangedExecute()
        //{
        //    if (SelectedNationality != null && Nationalities.Any())
        //    {
        //        SelectedItem.NationalityId = SelectedNationality.Id;
        //        SetChangeTracker();
        //    }
        //}
    }
}
