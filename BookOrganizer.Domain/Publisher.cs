﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;

namespace BookOrganizer.Domain
{
    public class Publisher : IIdentifiable
    {
        private string _name;
        private string _logoPath;

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


        public string LogoPath
        {
            get => _logoPath;
            set
            {
                _logoPath = DomainHelpers.SetPicturePath(value, "PubLogos");
            }
        }

        public ICollection<Book> Books { get; set; }
    }
}
