using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookOrganizer.DA
{
    public interface IAuthorLookupDataService
    {
        Task<IEnumerable<LookupItem>> GetAuthorLookupAsync(string viewModelName);
    }
}
