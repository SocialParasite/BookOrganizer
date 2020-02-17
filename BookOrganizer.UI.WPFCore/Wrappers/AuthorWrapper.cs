using System;
using System.Collections.Generic;
using System.Text;
using BookOrganizer.Domain;
using BookOrganizer.UI.WPFCore.ViewModels;

namespace BookOrganizer.UI.WPFCore.Wrappers
{
    public class AuthorWrapper : ViewModelBase
    {
        public readonly Author AuthorModel;

        public AuthorWrapper(Author authorModel)
        {
            AuthorModel = authorModel ?? throw new ArgumentNullException(nameof(authorModel));
        }

        public Guid Id { get => AuthorModel.Id; }

        public string FirstName
        {
            get => AuthorModel.FirstName;
            set
            {
                AuthorModel.FirstName = value;
                OnPropertyChanged();
            }
        }

        public string LastName
        {
            get => AuthorModel.LastName;
            set
            {
                AuthorModel.LastName = value;
                OnPropertyChanged();
            }
        }

        public DateTime? DateOfBirth
        {
            get => AuthorModel.DateOfBirth;
            set
            {
                AuthorModel.DateOfBirth = value;
                OnPropertyChanged();
            }
        }

        public string MugShotPath
        {
            get => AuthorModel.MugShotPath;
            set
            {
                AuthorModel.MugShotPath = DomainHelpers.SetPicturePath(value, "AuthorPics");

                OnPropertyChanged();
            }
        }

        public string Biography
        {
            get => AuthorModel.Biography;
            set { AuthorModel.Biography = value; OnPropertyChanged(); }
        }

        public Guid? NationalityId 
        {
            get => AuthorModel.NationalityId;
            set { AuthorModel.NationalityId = value; OnPropertyChanged(); }
        }
    }
}
