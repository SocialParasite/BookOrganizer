using BookOrganizer.Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookOrganizer.UI.WPF.Lookups
{
    public interface ILanguageLookupDataService
    {
        Task<IEnumerable<LookupItem>> GetLanguageLookupAsync();

        Task<Language> GetLanguageById(Guid languageId);
    }
}
