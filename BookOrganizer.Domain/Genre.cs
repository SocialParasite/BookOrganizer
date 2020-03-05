using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookOrganizer.Domain
{
    public class Genre : BaseDomainEntity, IIdentifiable
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }


        [Required]
        [MinLength(1, ErrorMessage = "Genres name should be at minimum 1 character long.")]
        [MaxLength(32, ErrorMessage = "Genres name should be maximum of 32 characters long.")]
        public string Name { get; set; }
        
        // Navigation properties
        public ICollection<BookGenres> BookLink { get; set; }
    }
}
