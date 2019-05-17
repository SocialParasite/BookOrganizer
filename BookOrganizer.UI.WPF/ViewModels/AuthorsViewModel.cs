using BookOrganizer.Domain;
using BookOrganizer.UI.WPF.Events;
using BookOrganizer.UI.WPF.Lookups;
using Prism.Commands;
using Prism.Events;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BookOrganizer.UI.WPF.ViewModels
{
    public class AuthorsViewModel : BaseViewModel<Author>, IAuthorsViewModel
    {
        private readonly IAuthorLookupDataService authorLookupDataService;

        public AuthorsViewModel(IEventAggregator eventAggregator,
                              IAuthorLookupDataService authorLookupDataService)
            : base(eventAggregator)
        {
            this.authorLookupDataService = authorLookupDataService
                ?? throw new ArgumentNullException(nameof(authorLookupDataService));

            //AuthorNameLabelMouseLeftButtonUpCommand =
            //    new DelegateCommand<Guid?>(OnAuthorNameLabelMouseLeftButtonUpExecute,
            //                               OnAuthorNameLabelMouseLeftButtonUpCanExecute);

            InitializeRepositoryAsync();
        }

        //public ICommand AuthorNameLabelMouseLeftButtonUpCommand { get; set; }

        //private bool OnAuthorNameLabelMouseLeftButtonUpCanExecute(Guid? id)
        //    => (id is null || id == Guid.Empty) ? false : true;

        //private void OnAuthorNameLabelMouseLeftButtonUpExecute(Guid? id)
        //{
        //    eventAggregator.GetEvent<OpenDetailViewEvent>()
        //                           .Publish(new OpenDetailViewEventArgs
        //                           {
        //                               Id = (Guid)id,
        //                               ViewModelName = nameof(AuthorDetailViewModel)
        //                           });
        //}

        public async override Task InitializeRepositoryAsync()
        {
            Items = await authorLookupDataService.GetAuthorLookupAsync();

            EntityCollection = Items.OrderBy(p => p.DisplayMember).ToList();
        }
    }
}
