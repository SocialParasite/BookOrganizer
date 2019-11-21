using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;

namespace BookOrganizer.Domain
{
    public class Book : BaseDomainEntity, IIdentifiable
    {
        public Book()
        {
            AuthorsLink = new ObservableCollection<BookAuthors>();
            ReadDates = new ObservableCollection<BooksReadDate>();
            BooksSeries = new ObservableCollection<BooksSeries>();
        }

        private string _title;
        private int _releaseYear;
        private int _pageCount;
        private int _wordCount;
        private string _iSBN;
        private string _bookCoverPicturePath;
        private bool _isRead;
        private string _description;

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        [MinLength(1, ErrorMessage = "Books title should be at minimum 1 character long.")]
        [MaxLength(256, ErrorMessage = "Books title should be maximum of 256 characters long.")]
        public string Title
        {
            get => _title;
            set
            {
                if (value is null || value == string.Empty || value.Length < 1 || value.Length > 256)
                    throw new ArgumentOutOfRangeException(nameof(Title), "Books title should be 1-256 characters long.");

                _title = value;
                OnPropertyChanged();
            }
        }

        [Range(1, 2500, ErrorMessage = "Fairytales older than year 1 shall not be permitted")]
        public int ReleaseYear
        {
            get => _releaseYear;
            set
            {
                if (value > 0 && value <= 2500)
                {
                    _releaseYear = value;
                    OnPropertyChanged();
                }
                else
                    throw new ArgumentOutOfRangeException(nameof(ReleaseYear), "Release year should be between 1 and 2500.");
            }
        }

        [Range(1, 10_000)]
        public int PageCount
        {
            get => _pageCount;
            set
            {
                if (value > 0 && value < 10_001)
                {
                    _pageCount = value;
                    OnPropertyChanged();
                }
                else
                    throw new ArgumentOutOfRangeException(nameof(PageCount), "Page count should be between 1 and 10 000.");
            }
        }

        [MaxLength(13)]
        public string ISBN
        {
            get => _iSBN;
            set
            {
                if (ValidateIsbn(value))
                {
                    _iSBN = value;
                    OnPropertyChanged();
                }
                else
                    throw new ArgumentOutOfRangeException(nameof(ISBN), "Isbn must be valid or empty.");
            }
        }

        [Range(0, int.MaxValue)]
        public int WordCount
        {
            get { return _wordCount; }
            set
            {
                if (value >= 0 && value < int.MaxValue)
                {
                    _wordCount = value;
                    OnPropertyChanged();
                }
                else
                    throw new ArgumentOutOfRangeException(nameof(WordCount), $"Word count must be between between 1 and {int.MaxValue}");
            }
        }

        public bool IsRead
        {
            get => _isRead;
            set
            {
                _isRead = value;
                OnPropertyChanged();
            }
        }

        public string Description
        {
            get => _description;
            set
            {
                _description = value;
                OnPropertyChanged();
            }
        }

        public string BookCoverPicturePath
        {
            get { return _bookCoverPicturePath; }
            set
            {
                _bookCoverPicturePath = DomainHelpers.SetPicturePath(value, "Covers");

                OnPropertyChanged();
            }
        }

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

        public bool ValidateIsbn(string isbn)
        {
            if (isbn is null || isbn is "") return true;

            var pattern = @"^(97(8|9))?\d{9}(\d|X)$";

            Regex rgx = new Regex(pattern, RegexOptions.IgnoreCase);

            return rgx.IsMatch(isbn);
        }

    }
}
