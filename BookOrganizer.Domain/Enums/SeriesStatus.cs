using System;

namespace BookOrganizer.Domain.Enums
{
    [Flags]
    public enum SeriesStatus
    {
        AllBooksRead = 1,
        AllBooksOwned = 2,
        PartlyRead = 4,
        PartlyOwned = 8,
        NoneRead = 16,
        NoneOwned = 32
    }
}