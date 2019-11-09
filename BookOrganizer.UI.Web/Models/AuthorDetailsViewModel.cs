using BookOrganizer.Data.Lookups;
using BookOrganizer.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BookOrganizer.UI.Web.Models
{
    public class AuthorDetailsViewModel : BaseDetailViewModel<Author>
    {
        public AuthorDetailsViewModel(Author selectedAuthor)
        {
            SelectedItem = selectedAuthor ?? throw new ArgumentNullException(nameof(selectedAuthor));
        }

        public string AuthorDOB
        {
            get
            {
                return SelectedItem.DateOfBirth != null
                    ? $"{SelectedItem.DateOfBirth:dd.MM.yyyy} ({(int)Math.Floor((DateTime.Now - (DateTime)SelectedItem.DateOfBirth).TotalDays / 365.25D)} years)"
                    : String.Empty;
            }
        }

        public string Nationality { get => SelectedItem.Nationality?.Name; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? DOB => SelectedItem.DateOfBirth;

        public List<LookupItem> Nationalities { get; set; }
    }
}
