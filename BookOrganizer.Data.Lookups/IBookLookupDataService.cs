using BookOrganizer.Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookOrganizer.Data.Lookups
{
    public interface IBookLookupDataService
    {
        Task<IEnumerable<LookupItem>> GetBookLookupAsync(string viewModelName);
        //Task<Book> GetBookById(Guid bookId);
    }
}
