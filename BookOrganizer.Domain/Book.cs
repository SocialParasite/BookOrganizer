using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BookOrganizer.Domain
{
    public class Book
    {
        private string _title;

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        [MinLength(1, ErrorMessage = "Books title should be at minimum 1 character long.")]
        [MaxLength(256, ErrorMessage = "Books title should be maximum of 256 characters long.")]
        public string Title
        {
            get => _title;
            set
            {
                if (value.Length < 1 || value.Length > 256)
                    throw new ArgumentOutOfRangeException(nameof(Title), "Books title should be 1-256 characters long.");

                _title = value;
            }
        }

    }
}
