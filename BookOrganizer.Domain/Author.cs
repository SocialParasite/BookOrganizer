using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookOrganizer.Domain
{
    public class Author : IIdentifiable
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [MinLength(1, ErrorMessage = "Authors first name should be at minimum 1 character long.")]
        [MaxLength(50, ErrorMessage = "Authors first name should be at maximum 50 character long.")]
        [Required]
        public string FirstName { get; set; }

        [MinLength(1, ErrorMessage = "Authors last name should be at minimum 1 character long.")]
        [MaxLength(50, ErrorMessage = "Authors last name should be at maximum 50 character long.")]
        [Required]
        public string LastName { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public string MugShotPath { get; set; }

        public string Biography { get; set; }

        public Guid? NationalityId { get; set; }

        // Navigation properties
        public ICollection<BookAuthors> BooksLink { get; set; }
        public Nationality Nationality { get; set; }
    }
}
