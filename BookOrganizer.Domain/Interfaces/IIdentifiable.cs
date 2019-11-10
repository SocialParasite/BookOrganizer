using System;

namespace BookOrganizer.Domain
{
    public interface IIdentifiable
    {
        Guid Id { get; set; }
    }
}
