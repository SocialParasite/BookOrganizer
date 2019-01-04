using BookOrganizer.Data.SqlServer;
using BookOrganizer.UI.WPF.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace BookOrganizer.UI.WPF.Lookups
{
    public class LookupDataService : IBookLookupDataService
    {
        private Func<BookOrganizerDbContext> _contextCreator;

        public LookupDataService(Func<BookOrganizerDbContext> contextCreator)
        {
            _contextCreator = contextCreator;
        }

        public async Task<IEnumerable<LookupItem>> GetBookLookupAsync()
        {
            var placeholderPic = $"{Path.GetDirectoryName((Assembly.GetExecutingAssembly().GetName().CodeBase)).Substring(6)}\\placeholder.png";

            using (var ctx = _contextCreator())
            {
                return await ctx.Books.AsNoTracking()
                  .Select(b =>
                  new LookupItem
                  {
                      Id = b.Id,
                      DisplayMember = $"{b.Title} ({b.ReleaseYear})",
                      Picture = b.BookCoverPicture ?? placeholderPic,
                      ViewModelName = nameof(BookDetailViewModel)
                  })
                  .ToListAsync();
            }
        }
    }
}
