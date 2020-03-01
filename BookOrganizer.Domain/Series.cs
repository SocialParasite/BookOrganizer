using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookOrganizer.Domain
{
    public class Series : IIdentifiable
    {
        public Series()
        {
            SeriesReadOrder = new ObservableCollection<SeriesReadOrder>();
        }

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        [MinLength(1, ErrorMessage = "Series name should be at minimum 1 character long.")]
        [MaxLength(256, ErrorMessage = "Series name should be maximum of 256 characters long.")]
        public string Name { get; set; }

        [Range(1, 1000)]
        public int NumberOfBooks { get; set; }

        public string PicturePath { get; set; }
        public string Description { get; set; }

        public ICollection<BooksSeries> BooksSeries { get; set; }

        public ICollection<SeriesReadOrder> SeriesReadOrder { get; set; }
    }
}
