using BookOrganizer.Data.Lookups;
using BookOrganizer.DA;
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
        private readonly INationalityLookupDataService nationalityLookupDataService;

        public AuthorController(IAuthorLookupDataService authorLookupDataService, 
            IRepository<Author> authorsRepository,
            INationalityLookupDataService nationalityLookupDataService)
        {
            this.authorLookupDataService = authorLookupDataService ?? throw new ArgumentNullException(nameof(authorLookupDataService));
            this.authorsRepository = authorsRepository ?? throw new ArgumentNullException(nameof(authorsRepository));
            this.nationalityLookupDataService = nationalityLookupDataService ?? throw new ArgumentNullException(nameof(nationalityLookupDataService));
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

        public async Task<IActionResult> Edit(Guid? Id)
        {
            if (ModelState.IsValid)
            {
                if (Id == null) throw new ArgumentNullException(nameof(Id));

                var author = await authorsRepository.GetSelectedAsync((Guid)Id);

                var nationalities = await nationalityLookupDataService.GetNationalityLookupAsync(nameof(AuthorDetailsViewModel));

                var vm = new AuthorDetailsViewModel(author);

                vm.Nationalities = nationalities.ToList();

                return View(vm); 
            }

            return Redirect("/");
        }
    }
}
