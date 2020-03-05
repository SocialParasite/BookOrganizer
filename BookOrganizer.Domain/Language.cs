using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookOrganizer.Domain
{
    public class Language : BaseDomainEntity, IIdentifiable
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        [MinLength(1, ErrorMessage = "Language name should be at minimum 1 character long.")]
        [MaxLength(32, ErrorMessage = "Language name should be maximum of 32 characters long.")]
        public string LanguageName { get; set; }
    }
}
