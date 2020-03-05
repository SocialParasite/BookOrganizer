using System;

namespace BookOrganizer.Domain.Services
{
    public class NationalityService : IDomainService<Nationality>
    {
        public IRepository<Nationality> Repository { get; }

        public NationalityService(IRepository<Nationality> repository)
        {
            Repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public Nationality CreateItem()
        {
            return new Nationality();
        }
    }
}
