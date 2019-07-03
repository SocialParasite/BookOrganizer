using BookOrganizer.Data.SqlServer;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookOrganizer.Data.Lookups
{
    public class LookupDataService : IBookLookupDataService, ILanguageLookupDataService,
                                     IPublisherLookupDataService, IAuthorLookupDataService,
                                     ISeriesLookupDataService, INationalityLookupDataService,
                                     IFormatLookupDataService, IGenreLookupDataService
    {
        private Func<BookOrganizerDbContext> _contextCreator;
        private readonly string placeholderPic;

        public LookupDataService(Func<BookOrganizerDbContext> contextCreator, string imagePath)
        {
            _contextCreator = contextCreator;
            placeholderPic = imagePath;
        }

        public async Task<IEnumerable<LookupItem>> GetBookLookupAsync(string viewModelName)
        {
            using (var ctx = _contextCreator())
            {
                return await ctx.Books
                    .AsNoTracking()
                    .Select(b =>
                        new LookupItem
                        {
                            Id = b.Id,
                            DisplayMember = $"{b.Title} ({b.ReleaseYear})",
                            Picture = b.BookCoverPicturePath ?? placeholderPic,
                            ViewModelName = viewModelName
                        })
                    .ToListAsync();
            }
        }

        public async Task<IEnumerable<LookupItem>> GetPublisherLookupAsync(string viewModelName)
        {
            using (var ctx = _contextCreator())
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
            using (var ctx = _contextCreator())
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
            using (var ctx = _contextCreator())
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
            using (var ctx = _contextCreator())
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
            using (var ctx = _contextCreator())
            {
                return await ctx.Series
                    .AsNoTracking()
                    .Select(s =>
                        new LookupItem
                        {
                            Id = s.Id,
                            DisplayMember = s.Name,
                            Picture = s.PicturePath ?? placeholderPic,
                            ViewModelName = viewModelName
                        })
                    .ToListAsync();
            }
        }

        public async Task<IEnumerable<LookupItem>> GetNationalityLookupAsync(string viewModelName)
        {
            using (var ctx = _contextCreator())
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
            using (var ctx = _contextCreator())
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
            using (var ctx = _contextCreator())
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
            using (var ctx = _contextCreator())
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
            using (var ctx = _contextCreator())
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
            using (var ctx = _contextCreator())
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
