using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookOrganizer.Data.Lookups
{
    public interface ISeriesLookupDataService
    {
        Task<IEnumerable<LookupItem>> GetSeriesLookupAsync(string viewModelName);
    }
}
