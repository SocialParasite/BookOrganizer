using BookOrganizer.Data.SqlServer;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookOrganizer.Domain;
using BookOrganizer.Domain.Enums;
using Microsoft.EntityFrameworkCore.Extensions.Internal;

namespace BookOrganizer.DA
{
    public class LookupDataService : IBookLookupDataService, ILanguageLookupDataService,
                                     IPublisherLookupDataService, IAuthorLookupDataService,
                                     ISeriesLookupDataService, INationalityLookupDataService,
                                     IFormatLookupDataService, IGenreLookupDataService
    {
        private readonly Func<BookOrganizerDbContext> contextCreator;
        private readonly string placeholderPic;

        public LookupDataService(Func<BookOrganizerDbContext> contextCreator, string imagePath)
        {
            this.contextCreator = contextCreator;
            placeholderPic = imagePath;
        }

        public async Task<IEnumerable<LookupItem>> GetBookLookupAsync(string viewModelName)
        {
            using (var ctx = contextCreator())
            {
                return await ctx.Books
                    .AsNoTracking()
                    .Include(b => b.FormatLink)
                    .AsAsyncEnumerable()
                    .Select(b =>
                        new LookupItem
                        {
                            Id = b.Id,
                            DisplayMember = $"{b.Title} ({b.ReleaseYear})",
                            Picture = b.BookCoverPicturePath ?? placeholderPic,
                            ViewModelName = viewModelName,
                            ItemStatus = CheckBookStatus(b.IsRead, b.FormatLink.Count > 0)
                        })
                    .ToList();
            }

            BookStatus CheckBookStatus(bool read, bool owned)
            {
                if (read && owned)
                    return BookStatus.Read | BookStatus.Owned;

                if (read)
                    return BookStatus.Read;

                if (owned)
                    return BookStatus.Owned;

                return BookStatus.None;
            }
        }

        public async Task<IEnumerable<LookupItem>> GetPublisherLookupAsync(string viewModelName)
        {
            using (var ctx = contextCreator())
            {
                return await ctx.Publishers
                    .AsNoTracking()
                    .OrderBy(p => p.Name)
                    .Select(p =>
                        new LookupItem
                        {
                            Id = p.Id,
                            DisplayMember = p.Name,
                            Picture = p.LogoPath ?? placeholderPic,
                            ViewModelName = viewModelName
                        })
                    .ToListAsync();
            }
        }

        public async Task<IEnumerable<LookupItem>> GetAuthorLookupAsync(string viewModelName)
        {
            using (var ctx = contextCreator())
            {
                return await ctx.Authors
                    .AsNoTracking()
                    .OrderBy(a => a.LastName)
                    .Select(a =>
                        new LookupItem
                        {
                            Id = a.Id,
                            DisplayMember = $"{a.LastName}, {a.FirstName}",
                            Picture = a.MugShotPath ?? placeholderPic,
                            ViewModelName = viewModelName
                        })
                    .ToListAsync();
            }
        }

        public async Task<IEnumerable<LookupItem>> GetLanguageLookupAsync(string viewModelName)
        {
            using (var ctx = contextCreator())
            {
                return await ctx.Languages
                    .AsNoTracking()
                    .OrderBy(l => l.LanguageName)
                    .Select(l =>
                        new LookupItem
                        {
                            Id = l.Id,
                            DisplayMember = l.LanguageName,
                            Picture = null,
                            ViewModelName = viewModelName
                        })
                    .ToListAsync();
            }
        }

        public async Task<Guid> GetLanguageId()
        {
            using (var ctx = contextCreator())
            {
                return await ctx.Languages
                    .AsNoTracking()
                    .OrderBy(l => l.LanguageName)
                    .Select(l => l.Id)
                    .FirstOrDefaultAsync();
            }
        }

        public async Task<IEnumerable<LookupItem>> GetSeriesLookupAsync(string viewModelName)
        {
            using (var ctx = contextCreator())
            {
                return await ctx.Series
                    .Include(s => s.SeriesReadOrder)
                    .ThenInclude(s => s.Book)
                    .ThenInclude(b => b.FormatLink)
                    .AsNoTracking()
                    .AsAsyncEnumerable()
                    .Select(s =>
                        new LookupItem
                        {
                            Id = s.Id,
                            DisplayMember = s.Name,
                            Picture = s.PicturePath ?? placeholderPic,
                            ViewModelName = viewModelName,
                            ItemStatus = CheckSeriesStatus(s)
                        })
                    .ToList();
            }


            SeriesStatus CheckSeriesStatus(Series series)
            {
                var readStatus = series.SeriesReadOrder.All(b => b.Book.IsRead);

                bool partlyRead = false;
                bool partlyOwned = false;

                if (!readStatus) partlyRead = series.SeriesReadOrder.Any(b => b.Book.IsRead);

                var owned = series.SeriesReadOrder.All(b => b.Book.FormatLink.Count > 0);

                if (!readStatus) partlyOwned = series.SeriesReadOrder.Any(b => b.Book.FormatLink.Count > 0);

                if (readStatus && owned)
                    return SeriesStatus.AllOwnedAllRead;

                if (!partlyOwned && !partlyRead)
                    return SeriesStatus.NoneOwnedNoneRead;

                if (!owned && partlyRead)
                    return SeriesStatus.NoneOwnedPartlyRead;

                if (readStatus && !owned)
                    return SeriesStatus.NoneOwnedAllRead;

                if (!partlyRead && partlyOwned)
                    return SeriesStatus.PartlyOwnedNoneRead;


                if (partlyRead && partlyOwned)
                    return SeriesStatus.PartlyOwnedPartlyRead;

                if (readStatus && partlyOwned)
                    return SeriesStatus.PartlyOwnedAllRead;


                if (owned && !partlyRead)
                    return SeriesStatus.AllOwnedPartlyRead;

                if (owned && partlyRead)
                    return SeriesStatus.AllOwnedPartlyRead;

                return SeriesStatus.None;
            }
        }

        public async Task<IEnumerable<LookupItem>> GetNationalityLookupAsync(string viewModelName)
        {
            using (var ctx = contextCreator())
            {
                return await ctx.Nationalities
                    .AsNoTracking()
                    .OrderBy(n => n.Name)
                    .Select(n =>
                        new LookupItem
                        {
                            Id = n.Id,
                            DisplayMember = n.Name,
                            Picture = null,
                            ViewModelName = viewModelName
                        })
                    .ToListAsync();
            }
        }

        public async Task<Guid> GetNationalityId()
        {
            using (var ctx = contextCreator())
            {
                return await ctx.Nationalities
                    .AsNoTracking()
                    .OrderBy(n => n.Name)
                    .Select(n => n.Id)
                    .FirstOrDefaultAsync();
            }
        }

        public async Task<IEnumerable<LookupItem>> GetFormatLookupAsync(string viewModelName)
        {
            using (var ctx = contextCreator())
            {
                return await ctx.Formats
                    .AsNoTracking()
                    .OrderBy(f => f.Name)
                    .Select(f =>
                        new LookupItem
                        {
                            Id = f.Id,
                            DisplayMember = f.Name,
                            Picture = null,
                            ViewModelName = viewModelName
                        })
                    .ToListAsync();
            }
        }

        public async Task<Guid> GetFormatId()
        {
            using (var ctx = contextCreator())
            {
                return await ctx.Formats
                    .AsNoTracking()
                    .OrderBy(f => f.Name)
                    .Select(f => f.Id)
                    .FirstOrDefaultAsync();
            }
        }

        public async Task<IEnumerable<LookupItem>> GetGenreLookupAsync(string viewModelName)
        {
            using (var ctx = contextCreator())
            {
                return await ctx.Genres
                    .AsNoTracking()
                    .OrderBy(f => f.Name)
                    .Select(f =>
                        new LookupItem
                        {
                            Id = f.Id,
                            DisplayMember = f.Name,
                            Picture = null,
                            ViewModelName = viewModelName
                        })
                    .ToListAsync();
            }
        }

        public async Task<Guid> GetGenreId()
        {
            using (var ctx = contextCreator())
            {
                return await ctx.Genres
                    .AsNoTracking()
                    .OrderBy(f => f.Name)
                    .Select(f => f.Id)
                    .FirstOrDefaultAsync();
            }
        }

    }
}
