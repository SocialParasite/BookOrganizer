using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookOrganizer.Domain
{
    public interface IPublisherLookupDataService
    {
        Task<IEnumerable<LookupItem>> GetPublisherLookupAsync(string viewModelName);
    }
}
