using BookOrganizer.Data.Lookups;
using BookOrganizer.Data.Repositories;
using BookOrganizer.Domain;
using BookOrganizer.UI.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BookOrganizer.UI.Web.Controllers
{
    public class AuthorController : Controller
    {
        private readonly IAuthorLookupDataService authorLookupDataService;
        private readonly IRepository<Author> authorsRepository;

        public AuthorController(IAuthorLookupDataService authorLookupDataService, IRepository<Author> authorsRepository)
        {
            this.authorLookupDataService = authorLookupDataService ?? throw new ArgumentNullException(nameof(authorLookupDataService));
            this.authorsRepository = authorsRepository ?? throw new ArgumentNullException(nameof(authorsRepository));
        }

        public async Task<string> Index()
        {
            var authors = await authorLookupDataService.GetAuthorLookupAsync(nameof(AuthorsViewModel));

            return authors.FirstOrDefault().DisplayMember;
        }

        public async Task<ViewResult> Details(Guid? authorId)
        {
            if (authorId == null) authorId = Guid.Parse("5CCE28E1-5958-E811-A2D3-FCAA1497E0B4");
            var author = await authorsRepository.GetSelectedAsync((Guid)authorId);
            var vm = new AuthorDetailsViewModel() { FirstName = author.FirstName, LastName = author.LastName };

            return View(vm);
        }
    }
}
