using System;

namespace BookOrganizer.Domain
{
    public sealed class MonthlyReadsReport
    {
        public string Title { get; set; }
        public DateTime ReadDate { get; set; }
    }
}
