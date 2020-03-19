using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookOrganizer.Domain
{
    public interface IAuthorLookupDataService
    {
        Task<IEnumerable<LookupItem>> GetAuthorLookupAsync(string viewModelName);
    }
}
