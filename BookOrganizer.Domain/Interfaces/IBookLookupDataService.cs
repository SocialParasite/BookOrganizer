using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookOrganizer.Domain
{
    public interface IBookLookupDataService
    {
        Task<IEnumerable<LookupItem>> GetBookLookupAsync(string viewModelName);
    }
}
