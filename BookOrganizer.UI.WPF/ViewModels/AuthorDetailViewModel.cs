using BookOrganizer.Domain;
using BookOrganizer.UI.WPF.Repositories;
using BookOrganizer.UI.WPF.Services;
using Prism.Commands;
using Prism.Events;
using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BookOrganizer.UI.WPF.ViewModels
{
    public class AuthorDetailViewModel : BaseDetailViewModel<Author>, IAuthorDetailViewModel
    {
        public AuthorDetailViewModel(IEventAggregator eventAggregator,
            IMetroDialogService metroDialogService,
            IRepository<Author> authorRepo)
            : base(eventAggregator, metroDialogService)
        {
            Repository = authorRepo ?? throw new ArgumentNullException(nameof(authorRepo));

            AddAuthorPictureCommand = new DelegateCommand(OnAddAuthorPictureExecute);
        }

        public ICommand AddAuthorPictureCommand { get; set; }

        private void OnAddAuthorPictureExecute()
        {
            throw new NotImplementedException();
        }

        public async override Task LoadAsync(Guid id)
        {
            SelectedItem = await Repository.GetSelectedAsync(id) ?? null;

            Id = id;

            if (Id != Guid.Parse("00000000-0000-0000-0000-000000000000"))
            {
                TabTitle = $"{SelectedItem.LastName}, {SelectedItem.FirstName}";

            }
        }
    }
}
