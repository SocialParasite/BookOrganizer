using System;
using System.Collections.Generic;
using System.Text;

namespace BookOrganizer.Domain
{
    public class AnnualBookStatisticsInRangeReport
    {
        public int Year { get; set; }
        public int TotalNumberOfBooksRead { get; set; }
        public int ShortestBookRead { get; set; }
        public int LongestBookRead { get; set; }
        public int TotalPagesRead { get; set; }
        public int AveragePagesReadMonthly { get; set; }
        public int AverageBookLength { get; set; }
    }
}
