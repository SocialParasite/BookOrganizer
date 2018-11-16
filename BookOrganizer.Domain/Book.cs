using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace BookOrganizer.Domain
{
    public class Book
    {
        private string _title;
        private int _releaseYear;
        private int _pageCount;
        private string _iSBN;
        private string _bookCoverPicture;

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
            }
        }

        [Range(1, 2500, ErrorMessage = "Fairytales older than year 1 shall not be permitted")]
        public int ReleaseYear
        {
            get => _releaseYear;
            set
            {
                if (value > 0 && value <= 2500)
                    _releaseYear = value;
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
                    _pageCount = value;
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
                    _iSBN = value;
                else
                    throw new ArgumentOutOfRangeException(nameof(ISBN), "Isbn must be valid or empty.");
            }
        }

        public bool IsRead { get; set; }

        private bool ValidateIsbn(string isbn)
        {
            if (isbn is null || isbn is "") return true;

            var pattern = @"^(97(8|9))?\d{9}(\d|X)$";

            Regex rgx = new Regex(pattern, RegexOptions.IgnoreCase);

            return rgx.IsMatch(isbn);
        }

        public string Description { get; set; }

        public string BookCoverPicture
        {
            get { return _bookCoverPicture; }
            set
            {
                var pictureName = Path.GetFileName(value);

                var envPath = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
                var folder = @"\BookOrganizer\Covers\";
                var fullPath = $"{envPath}{folder}{pictureName}";

                fullPath = Path.GetFullPath(fullPath);
                _bookCoverPicture = fullPath;

                try
                {
                    if (Directory.Exists(Path.GetDirectoryName(fullPath))) return;

                    Directory.CreateDirectory(Path.GetDirectoryName(fullPath));
                }
                catch (Exception ex)
                {
                    throw; //TODO:
                }
            }
        }

        //public Guid LanguageId { get; set; }

        // Navigation properties
        //public ICollection<Author> Authors { get; set; }
        public ICollection<BookAuthors> AuthorsLink { get; set; }

        public Language Language { get; set; }
    }
}
