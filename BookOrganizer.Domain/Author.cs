﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BookOrganizer.Domain
{
    public class Author
    {
        private string _firstName;
        private string _lastName;

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [MinLength(1, ErrorMessage = "Authors first name should be at minimum 1 character long.")]
        [MaxLength(50, ErrorMessage = "Authors first name should be at maximum 50 character long.")]
        [Required]
        public string FirstName
        {
            get => _firstName;
            set
            {
                if(value is null || value == string.Empty || value.Length < 1 || value.Length > 50)
                    throw new ArgumentOutOfRangeException(nameof(FirstName), "Authors first name should be 1-50 characters long.");

                _firstName = value;
            }
        }

        [MinLength(1, ErrorMessage = "Authors last name should be at minimum 1 character long.")]
        [MaxLength(50, ErrorMessage = "Authors last name should be at maximum 50 character long.")]
        [Required]
        public string LastName
        {
            get => _lastName;
            set
            {
                if (value is null || value == string.Empty || value.Length < 1 || value.Length > 50)
                    throw new ArgumentOutOfRangeException(nameof(LastName), "Authors last name should be 1-50 characters long.");
                _lastName = value;
            }
        }

        // Navigation properties
        //public ICollection<Book> Books { get; set; }
        public ICollection<BookAuthors> BooksLink { get; set; }
    }
}
