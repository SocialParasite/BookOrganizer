using System;

namespace BookOrganizer.Domain.Enums
{
    [Flags]
    public enum SeriesStatus
    {
        None = 0,
        AllBooksRead = 1,
        AllBooksOwned = 2,
        PartlyRead = 4,
        PartlyOwned = 8,
        NoneRead = 16,
        NoneOwned = 32,

        NoneOwnedNoneRead = NoneOwned | NoneRead,
        NoneOwnedPartlyRead = NoneOwned | PartlyRead,
        NoneOwnedAllRead = NoneOwned | AllBooksRead,

        PartlyOwnedNoneRead = PartlyOwned | NoneRead,
        PartlyOwnedPartlyRead = PartlyOwned | PartlyRead,
        PartlyOwnedAllRead = PartlyOwned | AllBooksRead,

        AllOwnedNoneRead = AllBooksOwned | NoneRead,
        AllOwnedPartlyRead = AllBooksOwned | PartlyRead,
        AllOwnedAllRead = AllBooksOwned | AllBooksRead
    }
}