using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookOrganizer.Domain
{
    public class Nationality : BaseDomainEntity, IIdentifiable
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        [MinLength(1, ErrorMessage = "Nations name should be at minimum 1 character long.")]
        [MaxLength(32, ErrorMessage = "Nations name should be maximum of 32 characters long.")]
        public string Name { get; set; }

        // Navigational properties
        public ICollection<Author> Authors { get; set; }
    }
}
