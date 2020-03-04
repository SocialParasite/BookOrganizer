using System;
using System.Collections.Generic;
using System.Text;

namespace BookOrganizer.Domain.Services
{
    public class FormatService : IDomainService<Format>
    {
        public IRepository<Format> Repository { get; }

        public FormatService(IRepository<Format> repository)
        {
            Repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public Format CreateItem()
        {
            return new Format();
        }
    }
}
