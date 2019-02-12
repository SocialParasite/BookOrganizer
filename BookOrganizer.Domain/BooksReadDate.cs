using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookOrganizer.Domain
{
    public class BooksReadDate
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public DateTime ReadDate { get; set; }
        public Guid BookId { get; set; }

        // Navigation properties
        //public Book Book { get; set; }

    }
}
