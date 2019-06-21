using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BookOrganizer.Domain
{
    public class Format : BaseDomainEntity, IIdentifiable
    {
        private string _name;
        private string _abbreveation;

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }


        [Required]
        [MinLength(1, ErrorMessage = "Format name should be at minimum 1 character long.")]
        [MaxLength(32, ErrorMessage = "Format name should be maximum of 32 characters long.")]
        public string Name
        {
            get => _name;
            set
            {
                if (value is null || value == string.Empty || value.Length < 1 || value.Length > 32)
                    throw new ArgumentOutOfRangeException(nameof(Name), "Format name should be 1-32 characters long.");

                _name = value;
            }
        }

        //[Required]
        //[MinLength(1, ErrorMessage = "Format abbreveation should be at minimum 1 character long.")]
        //[MaxLength(16, ErrorMessage = "Format abbreveation should be maximum of 16 characters long.")]
        //public string Abbreveation
        //{
        //    get => _abbreveation;
        //    set
        //    {
        //        if (value is null || value == string.Empty || value.Length < 1 || value.Length > 16)
        //            throw new ArgumentOutOfRangeException(nameof(Abbreveation), "Format name should be 1-16 characters long.");

        //        _abbreveation = value;
        //    }
        //}

        // Navigation properties
        public ICollection<BooksFormats> BookLink { get; set; }
    }
}
