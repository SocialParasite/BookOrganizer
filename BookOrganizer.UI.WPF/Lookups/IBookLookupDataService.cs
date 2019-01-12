﻿using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookOrganizer.UI.WPF.Lookups
{
    public interface IBookLookupDataService
    {
        Task<IEnumerable<LookupItem>> GetBookLookupAsync();
    }
}
