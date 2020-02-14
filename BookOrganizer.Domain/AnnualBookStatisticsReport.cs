using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.ComponentModel.DataAnnotations;

namespace BookOrganizer.Domain
{
    [NotMapped]
    public class AnnualBookStatisticsReport
    {
        [Key]
        public string MonthName { get; set; }
        public int TotalNumberOfBooksRead { get; set; }
        public int ShortestBookRead { get; set; }
        public int LongestBookRead { get; set; }
        public int TotalPagesRead { get; set; }
        public int AveragePagesReadDaily { get; set; }
        public int AverageBookLength { get; set; }
    }
}
