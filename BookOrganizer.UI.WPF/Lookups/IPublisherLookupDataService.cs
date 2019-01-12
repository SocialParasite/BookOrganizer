using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookOrganizer.UI.WPF.Lookups
{
    public interface IPublisherLookupDataService
    {
        Task<IEnumerable<LookupItem>> GetPublisherLookupAsync();
    }
}
