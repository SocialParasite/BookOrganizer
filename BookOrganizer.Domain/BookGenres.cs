using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BookOrganizer.Domain
{
    public class BookGenres
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public Guid BookId { get; set; }
        public Guid GenreId { get; set; }

        // Navigation properties
        public Book Book { get; set; }
        public Genre Genre { get; set; }
    }
}
