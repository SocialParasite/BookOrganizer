using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookOrganizer.DA
{
    public interface INationalityLookupDataService
    {
        Task<IEnumerable<LookupItem>> GetNationalityLookupAsync(string viewModelName);

        Task<Guid> GetNationalityId();
    }
}
