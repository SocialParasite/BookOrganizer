using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookOrganizer.Domain
{
    public interface IGenreLookupDataService
    {
        Task<IEnumerable<LookupItem>> GetGenreLookupAsync(string viewModelName);
        Task<Guid> GetGenreId();
    }
}
