using BookOrganizer.Domain;
using System;
using System.Threading.Tasks;

namespace BookOrganizer.Data.Repositories
{
    public interface ISeriesRepository : IRepository<Series>
    {
        Task<Book> GetBookById(Guid bookId);
    }
}
