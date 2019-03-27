using BookOrganizer.Domain;
using BookOrganizer.UI.WPF.Events;
using BookOrganizer.UI.WPF.Lookups;
using Prism.Commands;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BookOrganizer.UI.WPF.ViewModels
{
    public class AuthorsViewModel : BaseViewModel<Author>, IAuthorsViewModel
    {
        private readonly IEventAggregator eventAggregator;
        private readonly IAuthorLookupDataService authorLookupDataService;
        private OpenDetailViewEventArgs selectedAuthor;

        public AuthorsViewModel(IEventAggregator eventAggregator,
                              IAuthorLookupDataService authorLookupDataService)
        {
            this.authorLookupDataService = authorLookupDataService
                ?? throw new ArgumentNullException(nameof(authorLookupDataService));
            this.eventAggregator = eventAggregator ?? throw new ArgumentNullException(nameof(eventAggregator));

            AuthorNameLabelMouseLeftButtonUpCommand =
                new DelegateCommand<Guid?>(OnAuthorNameLabelMouseLeftButtonUpExecute,
                                           OnAuthorNameLabelMouseLeftButtonUpCanExecute);
            AddNewAuthorCommand = new DelegateCommand(OnAddNewAuthorExecute);

            InitializeRepositoryAsync();
        }

        public ICommand AuthorNameLabelMouseLeftButtonUpCommand { get; set; }
        public ICommand AddNewAuthorCommand { get; set; }

        public OpenDetailViewEventArgs SelectedAuthor
        {
            get => selectedAuthor;
            set
            {
                selectedAuthor = value;
                OnPropertyChanged();
                if (selectedAuthor != null)
                {
                    eventAggregator.GetEvent<OpenDetailViewEvent>()
                                   .Publish(selectedAuthor);
                }
            }
        }

        private void OnAddNewAuthorExecute()
            => SelectedAuthor = new OpenDetailViewEventArgs { Id = new Guid(), ViewModelName = nameof(AuthorDetailViewModel) };

        private bool OnAuthorNameLabelMouseLeftButtonUpCanExecute(Guid? id)
            => (id is null || id == Guid.Empty) ? false : true;

        private void OnAuthorNameLabelMouseLeftButtonUpExecute(Guid? id)
            => SelectedAuthor = new OpenDetailViewEventArgs { Id = (Guid)id, ViewModelName = nameof(AuthorDetailViewModel) };


        public async override Task InitializeRepositoryAsync()
        {
            Items = await authorLookupDataService.GetAuthorLookupAsync();

            EntityCollection = Items.OrderBy(p => p.DisplayMember).ToList();
        }
    }
}
