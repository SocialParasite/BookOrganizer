using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookOrganizer.Domain
{
    public class Publisher
    {
        private string _name;

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        [MinLength(1, ErrorMessage = "Publishers name should be at minimum 1 character long.")]
        [MaxLength(64, ErrorMessage = "Publishers name should be maximum of 64 characters long.")]
        public string Name
        {
            get => _name;
            set
            {
                if (value is null || value == string.Empty || value.Length < 1 || value.Length > 64)
                    throw new ArgumentOutOfRangeException(nameof(Name), "Books title should be 1-64 characters long.");

                _name = value;
            }
        }

        public ICollection<Book> Books { get; set; }
    }
}
