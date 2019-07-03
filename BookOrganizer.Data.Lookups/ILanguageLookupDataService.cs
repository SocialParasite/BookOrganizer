using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookOrganizer.Data.Lookups
{
    public interface ILanguageLookupDataService
    {
        Task<IEnumerable<LookupItem>> GetLanguageLookupAsync(string viewModelName);
        Task<Guid> GetLanguageId();
    }
}
