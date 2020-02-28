using System;

namespace BookOrganizer.Domain.Services
{
    public class PublisherService : IPublisherService
    {
        public IRepository<Publisher> Repository { get; }

        public PublisherService(IRepository<Publisher> repository)
        {
            Repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public Publisher CreateItem()
        {
            return new Publisher();
        }
    }
}
