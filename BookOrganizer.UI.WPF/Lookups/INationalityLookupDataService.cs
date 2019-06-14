﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookOrganizer.UI.WPF.Lookups
{
    public interface INationalityLookupDataService
    {
        Task<IEnumerable<LookupItem>> GetNationalityLookupAsync();

        Task<Guid> GetNationalityId();
    }
}