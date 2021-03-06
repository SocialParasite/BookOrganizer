﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BookOrganizer.Domain
{
    public class BooksSeries
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public Guid BookId { get; set; }
        public Guid SeriesId { get; set; }

        // Navigation properties
        public Book Book { get; set; }
        public Series Series { get; set; }
    }
}
