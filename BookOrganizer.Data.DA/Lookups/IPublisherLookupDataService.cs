using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookOrganizer.DA
{
    public interface IPublisherLookupDataService
    {
        Task<IEnumerable<LookupItem>> GetPublisherLookupAsync(string viewModelName);
    }
}
