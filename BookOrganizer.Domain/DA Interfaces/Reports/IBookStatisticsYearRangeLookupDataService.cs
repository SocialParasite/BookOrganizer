using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookOrganizer.Domain
{
    public interface IBookStatisticsYearRangeLookupDataService
    {
        Task<IEnumerable<AnnualBookStatisticsInRangeReport>> GetAnnualBookStatisticsInRangeReportAsync(int? startYear, int? endYear);
    }
}
