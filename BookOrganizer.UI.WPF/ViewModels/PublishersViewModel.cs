using BookOrganizer.Domain;
using System;
using System.Threading.Tasks;

namespace BookOrganizer.UI.WPF.ViewModels
{
    public class PublishersViewModel : BaseViewModel<Publisher>, IPublishersViewModel
    {
        public override Task InitializeRepositoryAsync()
        {
            throw new NotImplementedException();
        }
    }
}
