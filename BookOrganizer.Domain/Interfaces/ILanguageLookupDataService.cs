using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookOrganizer.Domain
{
    public interface ILanguageLookupDataService
    {
        Task<IEnumerable<LookupItem>> GetLanguageLookupAsync(string viewModelName);
        Task<Guid> GetLanguageId();
    }
}
