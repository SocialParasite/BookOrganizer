using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookOrganizer.Domain
{
    public class Series : IIdentifiable
    {
        //private string _name;
        //private int _numberOfBooks;
        //private string picturePath;
        //private string description;

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
        //public string Name
        //{
        //    get => _name;
        //    set
        //    {
        //        if (value is null || value == string.Empty || value.Length < 1 || value.Length > 256)
        //            throw new ArgumentOutOfRangeException(nameof(Name), "Series name should be 1-256 characters long.");

        //        _name = value;

        //        OnPropertyChanged();
        //    }
        //}

        [Range(1, 1000)]
        public int NumberOfBooks { get; set; }
        //public int NumberOfBooks
        //{
        //    get => _numberOfBooks;
        //    set
        //    {
        //        if (value < 1 || value > 1000)
        //            throw new ArgumentOutOfRangeException(nameof(Name), "Number of books in a series should be 1-1000.");

        //        _numberOfBooks = value;
        //    }
        //}

        public string PicturePath { get; set; }
        public string Description { get; set; }
        //public string PicturePath
        //{
        //    get { return picturePath; }
        //    set { picturePath = DomainHelpers.SetPicturePath(value, "SeriesPictures"); OnPropertyChanged(); }
        //}

        //public string Description
        //{
        //    get { return description; }
        //    set { description = value; OnPropertyChanged(); }
        //}

        public ICollection<BooksSeries> BooksSeries { get; set; }

        public ICollection<SeriesReadOrder> SeriesReadOrder { get; set; }
    }
}
