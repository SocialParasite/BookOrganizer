using System;
using BookOrganizer.Domain;
using BookOrganizer.Domain.Services;

namespace BookOrganizer.UI.WPFCore.Wrappers
{
    public class BookWrapper : BaseWrapper<Book>
    {
        private readonly IDomainService<Book> domainService;

        public BookWrapper(Book model, IDomainService<Book> domainService) 
            : base(model) 
        {
            this.domainService = domainService ?? throw new ArgumentNullException(nameof(domainService));
        }

        public string Title
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }

        public int ReleaseYear
        {
            get { return GetValue<int>(); }
            set { SetValue(value); }
        }

        public string ISBN
        {
            get { return GetValue<string>(); }
            set
            {
                SetValue(value);
                if (!(domainService as BookService).ValidateIsbn(value))
                {
                    AddError(nameof(ISBN), "Not valid ISBN code!");
                }
            }
        }

        public int WordCount
        {
            get { return GetValue<int>(); }
            set { SetValue(value); }
        }

        public int PageCount
        {
            get { return GetValue<int>(); }
            set { SetValue(value); }
        }

        public bool IsRead
        {
            get { return GetValue<bool>(); }
            set { SetValue(value); }
        }

        public string Description
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }

        public string BookCoverPicturePath
        {
            get { return GetValue<string>(); }
            set
            {
                SetValue(DomainHelpers.SetPicturePath(value, "Covers"));
            }
        }

        public Guid LanguageId
        {
            get { return GetValue<Guid>(); }
            set { SetValue(value); }
        }

        public Guid PublisherId
        {
            get { return GetValue<Guid>(); }
            set { SetValue(value); }
        }
    }
}
