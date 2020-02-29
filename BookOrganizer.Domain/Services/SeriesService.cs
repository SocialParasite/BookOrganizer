using System;

namespace BookOrganizer.Domain.Services
{
    public class SeriesService : ISeriesService
    {
        public IRepository<Series> Repository { get; }

        public SeriesService(IRepository<Series> repository)
        {
            Repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public Series CreateItem()
        {
            return new Series();
        }
    }
}
