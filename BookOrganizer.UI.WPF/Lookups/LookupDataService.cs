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
    public class LookupDataService : IBookLookupDataService, ILanguageLookupDataService, IPublisherLookupDataService
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

        public async Task<IEnumerable<LookupItem>> GetLanguageLookupAsync()
        {
            using (var ctx = _contextCreator())
            {
                return await ctx.Languages.AsNoTracking()
                  .Select(l =>
                  new LookupItem
                  {
                      Id = l.Id,
                      DisplayMember = l.LanguageName,
                      Picture = null,
                      ViewModelName = null //nameof(LanguageDetailViewModel)
                  })
                  .ToListAsync();
            }
        }

        public async Task<IEnumerable<LookupItem>> GetPublisherLookupAsync()
        {
            using (var ctx = _contextCreator())
            {
                return await ctx.Publishers.AsNoTracking()
                  .Select(p =>
                  new LookupItem
                  {
                      Id = p.Id,
                      DisplayMember = p.Name,
                      Picture = null,
                      ViewModelName = null //nameof(PublisherDetailViewModel)
                  })
                  .ToListAsync();
            }
        }
    }
}
