using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookOrganizer.DA
{
    public interface IFormatLookupDataService
    {
        Task<IEnumerable<LookupItem>> GetFormatLookupAsync(string viewModelName);
        Task<Guid> GetFormatId();
    }
}
