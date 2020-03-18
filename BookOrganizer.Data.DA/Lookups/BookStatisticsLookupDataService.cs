using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BookOrganizer.Data.SqlServer;
using BookOrganizer.Domain;
using Microsoft.EntityFrameworkCore;

namespace BookOrganizer.Data.DA
{
    public class BookStatisticsLookupDataService
    {
        private Func<BookOrganizerDbContext> _contextCreator;

        public BookStatisticsLookupDataService(Func<BookOrganizerDbContext> contextCreator)
        {
            _contextCreator = contextCreator ?? throw new ArgumentNullException(nameof(contextCreator));
        }

        public async Task<IEnumerable<AnnualBookStatisticsReport>> GetAnnualBookStatisticsReportAsync(int year)
        {
            using (var ctx = _contextCreator())
            {
                return await ctx.Query<AnnualBookStatisticsReport>()
                    .AsNoTracking()
                    .FromSql($"EXEC GetBookStatisticsByYear {year}")
                    .ToListAsync();
            }
        }
    }
}
