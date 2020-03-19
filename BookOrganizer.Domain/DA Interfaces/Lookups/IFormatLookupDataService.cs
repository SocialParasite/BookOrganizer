using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookOrganizer.Domain
{
    public interface IFormatLookupDataService
    {
        Task<IEnumerable<LookupItem>> GetFormatLookupAsync(string viewModelName);
        Task<Guid> GetFormatId();
    }
}
