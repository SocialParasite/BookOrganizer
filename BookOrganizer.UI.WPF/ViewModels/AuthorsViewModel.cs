using BookOrganizer.Domain;
using BookOrganizer.DA;
using Prism.Events;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BookOrganizer.UI.WPF.ViewModels
{
    public class AuthorsViewModel : BaseViewModel<Author>
    {
        private readonly IAuthorLookupDataService authorLookupDataService;
        public AuthorsViewModel(IEventAggregator eventAggregator,
                              IAuthorLookupDataService authorLookupDataService)
            : base(eventAggregator)
        {
            this.authorLookupDataService = authorLookupDataService
                ?? throw new ArgumentNullException(nameof(authorLookupDataService));

            InitializeRepositoryAsync();
        }

        public async override Task InitializeRepositoryAsync()
        {
            Items = await authorLookupDataService.GetAuthorLookupAsync(nameof(AuthorDetailViewModel));

            EntityCollection = Items.OrderBy(p => p.DisplayMember).ToList();
        }
    }
}
