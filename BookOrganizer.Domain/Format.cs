﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookOrganizer.Domain
{
    public class Format : IIdentifiable
    {
        private string _name;

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }


        [Required]
        [MinLength(1, ErrorMessage = "Format name should be at minimum 1 character long.")]
        [MaxLength(32, ErrorMessage = "Format name should be maximum of 32 characters long.")]
        public string Name { get; set; }

        // Navigation properties
        public ICollection<BooksFormats> BookLink { get; set; }
    }
}
