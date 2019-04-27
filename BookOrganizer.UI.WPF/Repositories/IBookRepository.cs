using BookOrganizer.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookOrganizer.UI.WPF.Repositories
{
    public interface IBookRepository : IRepository<Book>
    {
        Task<Author> GetBookAuthorById(Guid authorId);
    }
}
