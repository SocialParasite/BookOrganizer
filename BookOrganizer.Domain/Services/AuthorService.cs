using System;

namespace BookOrganizer.Domain.Services
{
    public class AuthorService : IAuthorService
    {
        public IRepository<Author> Repository { get;  }
        public INationalityLookupDataService NationalityLookupDataService { get; }

        public AuthorService(IRepository<Author> repository, INationalityLookupDataService nationalityLookupDataService)
        {
            Repository = repository ?? throw new ArgumentNullException(nameof(repository));
            NationalityLookupDataService = nationalityLookupDataService ?? throw new ArgumentNullException(nameof(nationalityLookupDataService));
        }

        public Author CreateItem()
        {
            return new Author();
        }
    }
}
