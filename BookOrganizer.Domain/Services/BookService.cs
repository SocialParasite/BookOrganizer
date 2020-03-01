using System;
using System.Text.RegularExpressions;

namespace BookOrganizer.Domain.Services
{
    public class BookService : IBookService
    {
        public IRepository<Book> Repository { get; }
        public readonly ILanguageLookupDataService languageLookupDataService;
        public readonly IPublisherLookupDataService publisherLookupDataService;
        public readonly IAuthorLookupDataService authorLookupDataService;
        public readonly IFormatLookupDataService formatLookupDataService;
        public readonly IGenreLookupDataService genreLookupDataService;

        public BookService(IRepository<Book> repository,
            ILanguageLookupDataService languageLookupDataService,
            IPublisherLookupDataService publisherLookupDataService,
            IAuthorLookupDataService authorLookupDataService,
            IFormatLookupDataService formatLookupDataService,
            IGenreLookupDataService genreLookupDataService)
        {
            Repository = repository ?? throw new ArgumentNullException(nameof(repository));
            this.languageLookupDataService = languageLookupDataService;
            this.publisherLookupDataService = publisherLookupDataService;
            this.authorLookupDataService = authorLookupDataService;
            this.formatLookupDataService = formatLookupDataService;
            this.genreLookupDataService = genreLookupDataService;
        }

        public Book CreateItem()
        {
            return new Book();
        }

        public bool ValidateIsbn(string isbn)
        {
            if (isbn is null || isbn is "") return true;

            var pattern = @"^(97(8|9))?\d{9}(\d|X)$";

            Regex rgx = new Regex(pattern, RegexOptions.IgnoreCase);

            return rgx.IsMatch(isbn);
        }
    }
}
