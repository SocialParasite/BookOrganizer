using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BookOrganizer.Data.SqlServer;
using BookOrganizer.Domain;
using BookOrganizer.Domain.DA_Interfaces.Reports;
using Microsoft.EntityFrameworkCore;

namespace BookOrganizer.Data.DA
{
    public class ReportLookupDataService : IAnnualBookStatisticsLookupDataService,
        IBookStatisticsYearRangeLookupDataService, IMonthlyReadsLookupDataService
    {
        private readonly Func<BookOrganizerDbContext> contextCreator;

        public ReportLookupDataService(Func<BookOrganizerDbContext> contextCreator)
        {
            this.contextCreator = contextCreator ?? throw new ArgumentNullException(nameof(contextCreator));
        }

        public async Task<IEnumerable<AnnualBookStatisticsReport>> GetAnnualBookStatisticsReportAsync(int? year = null)
        {
            using (var ctx = contextCreator())
            {
                return await ctx.Query<AnnualBookStatisticsReport>()
                    .AsNoTracking()
                    .FromSql($"EXEC GetBookStatisticsByYear {year ?? DateTime.Now.Year}")
                    .ToListAsync();
            }
        }

        public async Task<IEnumerable<AnnualBookStatisticsInRangeReport>> GetAnnualBookStatisticsInRangeReportAsync(int? startYear, int? endYear)
        {
            using (var ctx = contextCreator())
            {
                return await ctx.Query<AnnualBookStatisticsInRangeReport>()
                    .AsNoTracking()
                    .FromSql($"EXEC GetAnnualBookStatisticsInRange {startYear ?? DateTime.Now.Year}, {endYear ?? DateTime.Now.Year}")
                    .ToListAsync();
            }
        }

        public async Task<IEnumerable<MonthlyReadsReport>> GetMonthlyReadsReportAsync(int? year = null, int? month = null)
        {
            using (var ctx = contextCreator())
            {
                return await ctx.Query<MonthlyReadsReport>()
                    .AsNoTracking()
                    .FromSql($"EXEC GetMonthlyReads {year ?? DateTime.Now.Year}, {month ?? DateTime.Now.Month}")
                    .ToListAsync();
            }
        }
    }
}
