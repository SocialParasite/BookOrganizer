using System;

namespace BookOrganizer.Domain.Services
{
    public class GenreService : IDomainService<Genre>
    {
        public IRepository<Genre> Repository { get; }

        public GenreService(IRepository<Genre> repository)
        {
            Repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public Genre CreateItem()
        {
            return new Genre();
        }
    }
}
