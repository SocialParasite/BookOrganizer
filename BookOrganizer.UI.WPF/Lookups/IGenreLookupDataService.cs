using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookOrganizer.UI.WPF.Lookups
{
    public interface IGenreLookupDataService
    {
        Task<IEnumerable<LookupItem>> GetGenreLookupAsync();
        Task<Guid> GetGenreId();
    }
}
