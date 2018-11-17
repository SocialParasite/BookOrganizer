﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BookOrganizer.Domain
{
    public class Nationality
    {
        private string _name;

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        [MinLength(1, ErrorMessage = "Nationality name should be at minimum 1 character long.")]
        [MaxLength(32, ErrorMessage = "Nationality name should be maximum of 32 characters long.")]
        public string Name
        {
            get => _name;
            set
            {
                if (value is null || value == string.Empty || value.Length < 1 || value.Length > 32)
                    throw new ArgumentOutOfRangeException(nameof(Name), "Nationality name should be 1-32 characters long.");

                _name = value;
            }
        }
    }
}