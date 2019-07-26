using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookOrganizer.Data.Lookups
{
    public interface IAuthorLookupDataService
    {
        Task<IEnumerable<LookupItem>> GetAuthorLookupAsync(string viewModelName);
    }
}
