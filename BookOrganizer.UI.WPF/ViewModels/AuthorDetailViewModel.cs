using BookOrganizer.Domain;
using BookOrganizer.UI.WPF.Repositories;
using BookOrganizer.UI.WPF.Services;
using Microsoft.Win32;
using Prism.Commands;
using Prism.Events;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media.Imaging;

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

        public string FirstName
        {
            get => firstName;
            set
            {
                firstName = value;
                OnPropertyChanged();
                SetTabTitle();
                SelectedItem.FirstName = value;
            }
        }

        public string LastName
        {
            get => lastName;
            set
            {
                lastName = value;
                OnPropertyChanged();
                SetTabTitle();
                SelectedItem.LastName = value;
            }
        }

        private void SetTabTitle()
        {
            TabTitle = $"{LastName}, {FirstName}";
        }

        private void OnAddAuthorPictureExecute()
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Title = "Select an image as an author picture";
            op.Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" +
                        "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
                        "Portable Network Graphic (*.png)|*.png";

            if (op.ShowDialog() == true)
            {
                var coverPic = new BitmapImage(new Uri(op.FileName));
                // TODO: testing...
                var coverPath = @"C:\\temp\\";

                SelectedItem.MugShotPath = coverPath + coverPic.UriSource.Segments.LastOrDefault();
            }
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
            SetDefaultAuthorPicIfNoneSet();

            //SelectedItem.PropertyChanged += (s, e) =>
            //{
            //    if (!HasChanges)
            //    {
            //        HasChanges = Repository.HasChanges();
            //    }
            //};
            void SetDefaultAuthorPicIfNoneSet()
            {
                if (SelectedItem.MugShotPath is null)
                    SelectedItem.MugShotPath =
                        $"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase).Substring(6)}\\placeholder.png";
            }
        }
    }
}
