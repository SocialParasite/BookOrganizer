using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BookOrganizer.Domain
{
    public class Website
    {
        private string _uri;

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        [MinLength(1, ErrorMessage = "Web address should be at minimum 1 character long.")]
        [MaxLength(256, ErrorMessage = "Web address should be maximum of 256 characters long.")]
        public string URI
        {
            get => _uri;
            set
            {
                if (value is null || value == string.Empty || value.Length < 1 || value.Length > 256)
                    throw new ArgumentOutOfRangeException(nameof(URI), "Web address should be 1-256 characters long.");

                _uri = value;
            }
        }

    }
}
