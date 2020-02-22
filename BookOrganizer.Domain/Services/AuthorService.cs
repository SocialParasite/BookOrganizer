using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookOrganizer.Domain.Services
{
    public class AuthorService : IDomainService<Author>
    {
        public IRepository<Author> Repository { get; set; }

        public AuthorService(IRepository<Author> repository)
        {
            Repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }
        
        public Author CreateNewAuthor()
        {
            return new Author();
        }
    }
}
