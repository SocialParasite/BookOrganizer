using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

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
        private readonly IFormatRepository formatRepository;
        private readonly IGenreRepository genreRepository;

        public BookService(IRepository<Book> repository,
            IFormatRepository formatRepository,
            IGenreRepository genreRepository,
            ILanguageLookupDataService languageLookupDataService,
            IPublisherLookupDataService publisherLookupDataService,
            IAuthorLookupDataService authorLookupDataService,
            IFormatLookupDataService formatLookupDataService,
            IGenreLookupDataService genreLookupDataService)
        {
            Repository = repository ?? throw new ArgumentNullException(nameof(repository));
            this.languageLookupDataService = languageLookupDataService ?? throw new ArgumentNullException(nameof(languageLookupDataService));
            this.publisherLookupDataService = publisherLookupDataService ?? throw new ArgumentNullException(nameof(publisherLookupDataService));
            this.authorLookupDataService = authorLookupDataService ?? throw new ArgumentNullException(nameof(authorLookupDataService));
            this.formatLookupDataService = formatLookupDataService ?? throw new ArgumentNullException(nameof(formatLookupDataService));
            this.genreLookupDataService = genreLookupDataService ?? throw new ArgumentNullException(nameof(genreLookupDataService));
            this.formatRepository = formatRepository ?? throw new ArgumentNullException(nameof(formatRepository));
            this.genreRepository = genreRepository ?? throw new ArgumentNullException(nameof(genreRepository));
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

        public Task AddNewBookFormat(Format format)
        {
            return formatRepository.AddNewFormatAsync(format);
        }

        public Task AddNewBookGenre(Genre genre)
        {
            return genreRepository.AddNewGenreAsync(genre);
        }
    }
}
