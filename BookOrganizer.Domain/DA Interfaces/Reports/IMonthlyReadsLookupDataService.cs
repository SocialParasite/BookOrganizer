using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookOrganizer.Domain.DA_Interfaces.Reports
{
    public interface IMonthlyReadsLookupDataService
    {
        Task<IEnumerable<MonthlyReadsReport>> GetMonthlyReadsReportAsync(int? year = null, int? month = null);
    }
}
