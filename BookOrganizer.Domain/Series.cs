using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BookOrganizer.Domain
{
    public class Series : IIdentifiable
    {
        private string _name;
        private int _numberOfBooks;
        private string picturePath;
        private string description;

        public Series()
        {
            BooksInSeries = new ObservableCollection<Book>();
            SeriesReadOrder = new ObservableCollection<SeriesReadOrder>();
        }

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        [MinLength(1, ErrorMessage = "Series name should be at minimum 1 character long.")]
        [MaxLength(256, ErrorMessage = "Series name should be maximum of 256 characters long.")]
        public string Name
        {
            get => _name;
            set
            {
                if (value is null || value == string.Empty || value.Length < 1 || value.Length > 256)
                    throw new ArgumentOutOfRangeException(nameof(Name), "Series name should be 1-256 characters long.");

                _name = value;
            }
        }

        [Range(1, 1000)]
        public int NumberOfBooks
        {
            get => _numberOfBooks;
            set
            {
                if (value < 1 || value > 1000)
                    throw new ArgumentOutOfRangeException(nameof(Name), "Number of books in a series should be 1-1000.");

                _numberOfBooks = value;
            }
        }

        public string PicturePath
        {
            get { return picturePath; }
            set { picturePath = DomainHelpers.SetPicturePath(value, "SeriesPictures"); }
        }

        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        // Navigation properties
        public ICollection<Book> BooksInSeries { get; set; }
        public ICollection<SeriesReadOrder> SeriesReadOrder { get; set; }
    }
}
