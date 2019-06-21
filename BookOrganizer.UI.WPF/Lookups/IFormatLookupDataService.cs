using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookOrganizer.UI.WPF.Lookups
{
    public interface IFormatLookupDataService
    {
        Task<IEnumerable<LookupItem>> GetFormatLookupAsync();
        Task<Guid> GetFormatId();
    }
}
