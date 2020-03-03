using System;

namespace BookOrganizer.Domain.Services
{
    public class BookGenreService : IDomainService<Genre>
    {
        public IRepository<Genre> Repository { get; }

        public BookGenreService(IRepository<Genre> repository)
        {
            Repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public Genre CreateItem()
        {
            return new Genre();
        }
    }
}
