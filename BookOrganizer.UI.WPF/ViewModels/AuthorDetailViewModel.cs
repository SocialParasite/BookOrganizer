using BookOrganizer.Domain;
using BookOrganizer.UI.WPF.Repositories;
using BookOrganizer.UI.WPF.Services;
using Prism.Commands;
using Prism.Events;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BookOrganizer.UI.WPF.ViewModels
{
    public class AuthorDetailViewModel : BaseDetailViewModel<Author>, IAuthorDetailViewModel
    {
        private string firstName;
        private string lastName;

        public AuthorDetailViewModel(IEventAggregator eventAggregator,
            IMetroDialogService metroDialogService,
            IRepository<Author> authorRepo)
            : base(eventAggregator, metroDialogService)
        {
            Repository = authorRepo ?? throw new ArgumentNullException(nameof(authorRepo));

            AddAuthorPictureCommand = new DelegateCommand(OnAddAuthorPictureExecute);

            SelectedItem = new Author();
        }

        public ICommand AddAuthorPictureCommand { get; set; }

        [MinLength(1, ErrorMessage = "Authors first name should be at minimum 1 character long.")]
        [MaxLength(50, ErrorMessage = "Authors first name should be at maximum 50 character long.")]
        [Required]
        public string FirstName
        {
            get => firstName;
            set
            {
                ValidatePropertyInternal(nameof(FirstName), value);
                firstName = value;
                OnPropertyChanged();
                SetTabTitle();
                ((DelegateCommand)SaveItemCommand).RaiseCanExecuteChanged();
                SelectedItem.FirstName = value;
            }
        }

        [MinLength(1, ErrorMessage = "Authors last name should be at minimum 1 character long.")]
        [MaxLength(50, ErrorMessage = "Authors last name should be at maximum 50 character long.")]
        [Required]
        public string LastName
        {
            get => lastName;
            set
            {
                ValidatePropertyInternal(nameof(LastName), value);
                lastName = value;
                OnPropertyChanged();
                SetTabTitle();
                ((DelegateCommand)SaveItemCommand).RaiseCanExecuteChanged();
                SelectedItem.LastName = value;
            }
        }

        private void SetTabTitle()
            => TabTitle = $"{LastName}, {FirstName}";

        private async void OnAddAuthorPictureExecute()
        {
            SelectedItem.MugShotPath = FileExplorerService.BrowsePicture() ?? SelectedItem.MugShotPath;
            await LoadAsync(this.Id);
        }

        public async override Task LoadAsync(Guid id)
        {
            SelectedItem = await Repository.GetSelectedAsync(id) ?? null;

            Id = id;

            if (Id != Guid.Parse("00000000-0000-0000-0000-000000000000"))
            {
                LastName = SelectedItem.LastName;
                FirstName = SelectedItem.FirstName;
                SetTabTitle();
            }
            else
                this.SwitchEditableStateExecute();

            SetDefaultAuthorPicIfNoneSet();

            SelectedItem.PropertyChanged += (s, e) =>
            {
                SetChangeTracker();
            };

            void SetDefaultAuthorPicIfNoneSet()
            {
                if (SelectedItem.MugShotPath is null)
                    SelectedItem.MugShotPath = FileExplorerService.GetImagePath();
            }
        }
    }
}
