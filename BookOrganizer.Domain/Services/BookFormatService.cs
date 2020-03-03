using System;

namespace BookOrganizer.Domain.Services
{
    public class BookFormatService : IDomainService<Format>
    {
        public IRepository<Format> Repository { get; }

        public BookFormatService(IRepository<Format> repository)
        {
            Repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public Format CreateItem()
        {
            return new Format();
        }
    }
}
