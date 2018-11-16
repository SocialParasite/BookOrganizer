﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BookOrganizer.Domain
{
    public class Language
    {
        private string _languageName;

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        [MinLength(1, ErrorMessage = "Language name should be at minimum 1 character long.")]
        [MaxLength(32, ErrorMessage = "Language name should be maximum of 64 characters long.")]
        public string LanguageName
        {
            get => _languageName;
            set
            {
                if (value is null || value == string.Empty || value.Length < 1 || value.Length > 64)
                    throw new ArgumentOutOfRangeException(nameof(LanguageName), "Language name should be 1-64 characters long.");

                _languageName = value;
            }
        }
    }
}
