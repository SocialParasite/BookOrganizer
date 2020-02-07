using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using BookOrganizer.DA;
using BookOrganizer.Domain;
using Prism.Events;
using Serilog;

namespace BookOrganizer.UI.WPFCore.ViewModels
{
    public class AuthorsViewModel : BaseViewModel<Author>
    {
        private readonly IAuthorLookupDataService authorLookupDataService;

        public AuthorsViewModel(IEventAggregator eventAggregator,
                                IAuthorLookupDataService authorLookupDataService,
                                ILogger logger)
            : base(eventAggregator, logger)
        {
            this.authorLookupDataService = authorLookupDataService
                ?? throw new ArgumentNullException(nameof(authorLookupDataService));

            Init();
        }

        private Task Init()
            => InitializeRepositoryAsync();

        public async override Task InitializeRepositoryAsync()
        {
            try
            {
                Items = await authorLookupDataService.GetAuthorLookupAsync(nameof(AuthorDetailViewModel)).ConfigureAwait(false);

                EntityCollection = Items.OrderBy(p => p.DisplayMember).ToList();

                logger.Information("GetAuthorLookupAsync() executed succesfully. {Timestamp}", DateTime.UtcNow);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                logger.Error("Message: {Message}\n\n Stack trace: {StackTrace}\n\n", ex.Message, ex.StackTrace);
            }
        }
    }
}
