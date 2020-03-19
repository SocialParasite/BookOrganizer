using System;
using System.Threading.Tasks;

namespace BookOrganizer.Domain
{
    public interface IBookRepository : IRepository<Book>
    {
        Task<Author> GetBookAuthorById(Guid authorId);
        Task<Format> GetBookFormatById(Guid formatId);

        Task<Genre> GetBookGenreById(Guid genreId);
    }
}
