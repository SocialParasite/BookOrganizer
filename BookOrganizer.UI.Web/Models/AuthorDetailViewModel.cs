using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BookOrganizer.Domain;

namespace BookOrganizer.UI.Web.Models
{
    public class AuthorDetailViewModel : BaseDetailViewModel<Author>
    {
        public AuthorDetailViewModel(Author selectedAuthor)
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

        public Tab ActiveTab { get; set; } //= Tab.Bibliography;

    }

    public enum Tab
    {
        Biography,
        Bibliography
    }
}
