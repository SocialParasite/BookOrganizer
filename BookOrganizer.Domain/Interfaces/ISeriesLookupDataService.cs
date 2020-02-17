using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookOrganizer.Domain
{
    public interface ISeriesLookupDataService
    {
        Task<IEnumerable<LookupItem>> GetSeriesLookupAsync(string viewModelName);
    }
}
