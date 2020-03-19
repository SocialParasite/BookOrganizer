using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookOrganizer.Domain
{
    public interface IAnnualBookStatisticsLookupDataService
    {
        Task<IEnumerable<AnnualBookStatisticsReport>> GetAnnualBookStatisticsReportAsync(int? year = null);
    }
}
