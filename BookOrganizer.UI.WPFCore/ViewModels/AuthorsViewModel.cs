using System;
using System.Linq;
using System.Threading.Tasks;
using BookOrganizer.DA;
using BookOrganizer.Domain;
using Prism.Events;

namespace BookOrganizer.UI.WPFCore.ViewModels
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

            Init();
        }

        public Task Init()
            => InitializeRepositoryAsync();

        public async override Task InitializeRepositoryAsync()
        {
            Items = await authorLookupDataService.GetAuthorLookupAsync(nameof(AuthorDetailViewModel)).ConfigureAwait(false);

            EntityCollection = Items.OrderBy(p => p.DisplayMember).ToList();
        }
    }
}
