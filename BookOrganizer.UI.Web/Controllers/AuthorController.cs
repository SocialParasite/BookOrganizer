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

        public async Task<IActionResult> Index()
        {
            var authors = await authorLookupDataService.GetAuthorLookupAsync(nameof(AuthorsViewModel));
            var vm = new AuthorsViewModel() { Authors = authors };
            return View(vm);
        }

        public async Task<IActionResult> Details(Guid? Id)
        {
            if (Id == null) throw new ArgumentNullException(nameof(Id));

            var author = await authorsRepository.GetSelectedAsync((Guid)Id);
            var vm = new AuthorDetailsViewModel(author); 

            return View(vm);
        }
    }
}
