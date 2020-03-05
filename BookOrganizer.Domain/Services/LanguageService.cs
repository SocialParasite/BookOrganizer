using System;

namespace BookOrganizer.Domain.Services
{
    public class LanguageService : IDomainService<Language>
    {
        public IRepository<Language> Repository { get; }

        public LanguageService(IRepository<Language> repository)
        {
            Repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public Language CreateItem()
        {
            return new Language();
        }
    }
}
