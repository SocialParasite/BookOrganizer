using System;
using BookOrganizer.Domain;

namespace BookOrganizer.UI.WPFCore.Wrappers
{
    public class AuthorWrapper : BaseWrapper<Author>
    {
        public AuthorWrapper(Author model) : base(model) { }
        
        public string FirstName
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }

        public string LastName
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }

        public DateTime? DateOfBirth
        {
            get { return GetValue<DateTime?>(); }
            set { SetValue(value); }
        }

        public string MugShotPath
        {
            get { return GetValue<string>(); }
            set 
            { 
                SetValue(DomainHelpers.SetPicturePath(value, "AuthorPics"));
            }
        }

        public string Biography
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }

        public Guid? NationalityId 
        {
            get { return GetValue<Guid?>(); }
            set { SetValue(value); }
        }
    }
}
