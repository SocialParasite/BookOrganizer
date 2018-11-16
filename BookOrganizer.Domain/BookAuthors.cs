using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookOrganizer.Domain
{
    public class BookAuthors
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public Guid BookId { get; set; }
        public Guid AuthorId { get; set; }

        // Navigation properties
        public Book Book { get; set; }
        public Author Author { get; set; }
    }
}
