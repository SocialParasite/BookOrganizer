using System;
using System.Threading.Tasks;

namespace BookOrganizer.Domain
{
    public interface ISeriesRepository : IRepository<Series>
    {
        Task<Book> GetBookById(Guid bookId);
    }
}
