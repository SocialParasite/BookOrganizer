using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookOrganizer.DA
{
    public interface ISeriesLookupDataService
    {
        Task<IEnumerable<LookupItem>> GetSeriesLookupAsync(string viewModelName);
    }
}
