using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BookOrganizer.Data.SqlServer;
using BookOrganizer.Domain;
using Microsoft.EntityFrameworkCore;

namespace BookOrganizer.Data.DA
{
    public class ReportLookupDataService
    {
        private Func<BookOrganizerDbContext> _contextCreator;

        public ReportLookupDataService(Func<BookOrganizerDbContext> contextCreator)
        {
            _contextCreator = contextCreator ?? throw new ArgumentNullException(nameof(contextCreator));
        }

        public async Task<IEnumerable<AnnualBookStatisticsReport>> GetAnnualBookStatisticsReportAsync(int? year = null)
        {
            using (var ctx = _contextCreator())
            {
                return await ctx.Query<AnnualBookStatisticsReport>()
                    .AsNoTracking()
                    .FromSql($"EXEC GetBookStatisticsByYear {year ?? DateTime.Now.Year}")
                    .ToListAsync();
            }
        }

        public async Task<IEnumerable<AnnualBookStatisticsInRangeReport>> GetAnnualBookStatisticsInRangeReportAsync(int? startYear, int? endYear)
        {
            using (var ctx = _contextCreator())
            {
                return await ctx.Query<AnnualBookStatisticsInRangeReport>()
                    .AsNoTracking()
                    .FromSql($"EXEC GetAnnualBookStatisticsInRange {startYear ?? DateTime.Now.Year}, {endYear ?? DateTime.Now.Year}")
                    .ToListAsync();
            }
        }
    }
}
