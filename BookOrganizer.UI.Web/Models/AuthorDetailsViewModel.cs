using BookOrganizer.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BookOrganizer.UI.Web.Models
{
    public class AuthorDetailsViewModel
    {
        public AuthorDetailsViewModel(Author selectedAuthor)
        {
            SelectedAuthor = selectedAuthor ?? throw new ArgumentNullException(nameof(selectedAuthor));
        }

        public Author SelectedAuthor { get; set; }

        public string AuthorPicture
        {
            get => $"~/Authorpics/{System.IO.Path.GetFileName(SelectedAuthor.MugShotPath)}";
        }

        public string AuthorDOB
        {
            get
            {
                return SelectedAuthor.DateOfBirth != null
                    ? $"{SelectedAuthor.DateOfBirth:dd.MM.yyyy} ({(int)Math.Floor((DateTime.Now - (DateTime)SelectedAuthor.DateOfBirth).TotalDays / 365.25D)} years)"
                    : String.Empty;
            }
        }

        public string Nationality { get => SelectedAuthor.Nationality?.Name; }
    }
}
