﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BookOrganizer.Domain
{
    public class Genre
    {
        private string _name;

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }


        [Required]
        [MinLength(1, ErrorMessage = "Genres name should be at minimum 1 character long.")]
        [MaxLength(32, ErrorMessage = "Genres name should be maximum of 32 characters long.")]
        public string Name
        {
            get => _name;
            set
            {
                if (value is null || value == string.Empty || value.Length < 1 || value.Length > 32)
                    throw new ArgumentOutOfRangeException(nameof(Name), "Genres name should be 1-32 characters long.");

                _name = value;
            }
        }

        // Navigation properties
        public ICollection<BookGenres> BookLink { get; set; }
    }
}