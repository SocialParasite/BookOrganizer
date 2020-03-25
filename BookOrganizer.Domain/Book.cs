using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookOrganizer.Domain
{
    public class Book : IIdentifiable
    {
        public Book()
        {
            AuthorsLink = new ObservableCollection<BookAuthors>();
            ReadDates = new ObservableCollection<BooksReadDate>();
            BooksSeries = new ObservableCollection<BooksSeries>();
        }

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        [MinLength(1, ErrorMessage = "Books title should be at minimum 1 character long.")]
        [MaxLength(256, ErrorMessage = "Books title should be maximum of 256 characters long.")]
        public string Title { get; set; }


        [Range(1, 2500, ErrorMessage = "Fairytales older than year 1 shall not be permitted")]
        public int  ReleaseYear { get; set; }

        [Range(1, 10_000)]
        public int PageCount { get; set; }

        [MaxLength(13)]
        public string ISBN { get; set; }

        [Range(0, int.MaxValue)]
        public int WordCount { get; set; }

        public bool IsRead { get; set; }

        public string Description { get; set; }

        [MaxLength(256)]
        public string BookCoverPicturePath { get; set; }

        public Guid LanguageId { get; set; }
        public Guid PublisherId { get; set; }

        // Navigation properties
        public ICollection<BookAuthors> AuthorsLink { get; set; }
        public Language Language { get; set; }
        public Publisher Publisher { get; set; }
        public ICollection<BooksReadDate> ReadDates { get; set; }
        public ICollection<BookGenres> GenreLink { get; set; }
        public ICollection<BooksFormats> FormatLink { get; set; }
        public ICollection<BooksSeries> BooksSeries { get; set; }
    }
}
