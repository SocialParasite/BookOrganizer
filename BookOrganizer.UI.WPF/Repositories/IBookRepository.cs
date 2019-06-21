using BookOrganizer.Domain;
using System;
using System.Threading.Tasks;

namespace BookOrganizer.UI.WPF.Repositories
{
    public interface IBookRepository : IRepository<Book>
    {
        Task<Author> GetBookAuthorById(Guid authorId);
        Task<Format> GetBookFormatById(Guid formatId);
    }
}
