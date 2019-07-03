using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookOrganizer.Data.Lookups
{
    public interface IPublisherLookupDataService
    {
        Task<IEnumerable<LookupItem>> GetPublisherLookupAsync(string viewModelName);
    }
}
