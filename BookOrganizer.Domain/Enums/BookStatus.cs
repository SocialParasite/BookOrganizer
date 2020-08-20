using System;

namespace BookOrganizer.Domain.Enums
{
    [Flags]
    public enum BookStatus
    {
        None = 0,
        Read = 1,
        Owned = 2
    }
}
