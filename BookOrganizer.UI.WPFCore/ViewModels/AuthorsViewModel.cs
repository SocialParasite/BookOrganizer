using System;
using System.Linq;
using System.Threading.Tasks;
using BookOrganizer.Domain;
using BookOrganizer.UI.WPFCore.DialogServiceManager;
using Prism.Events;
using Serilog;

namespace BookOrganizer.UI.WPFCore.ViewModels
{
    public class AuthorsViewModel : BaseViewModel<Author>
    {
        private readonly IAuthorLookupDataService authorLookupDataService;

        public AuthorsViewModel(IEventAggregator eventAggregator,
                                IAuthorLookupDataService authorLookupDataService,
                                ILogger logger,
                                IDialogService dialogService)
            : base(eventAggregator, logger, dialogService)
        {
            this.authorLookupDataService = authorLookupDataService
                ?? throw new ArgumentNullException(nameof(authorLookupDataService));

            Init();

            ViewModelType = nameof(AuthorDetailViewModel);
        }

        private Task Init()
            => InitializeRepositoryAsync();

        public async override Task InitializeRepositoryAsync()
        {
            try
            {
                Items = await authorLookupDataService.GetAuthorLookupAsync(nameof(AuthorDetailViewModel)).ConfigureAwait(false);

                EntityCollection = Items.OrderBy(p => p.DisplayMember).ToList();
            }
            catch (Exception ex)
            {
                var dialog = new NotificationViewModel("Exception", ex.Message);
                dialogService.OpenDialog(dialog);

                logger.Error("Message: {Message}\n\n Stack trace: {StackTrace}\n\n", ex.Message, ex.StackTrace);
            }
        }
    }
}
