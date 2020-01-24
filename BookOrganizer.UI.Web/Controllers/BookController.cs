using BookOrganizer.DA;
using BookOrganizer.Domain;
using BookOrganizer.UI.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BookOrganizer.UI.Web.Controllers
{
    public class BookController : Controller
    {
        private readonly IBookLookupDataService bookLookupDataService;
        private readonly IRepository<Book> booksRepository;

        public BookController(IBookLookupDataService bookLookupDataService, IRepository<Book> booksRepository)
        {
            this.bookLookupDataService = bookLookupDataService;
            this.booksRepository = booksRepository;
        }
        public async Task<IActionResult> Index()
        {
            var books = await bookLookupDataService.GetBookLookupAsync(nameof(BooksViewModel));
            var vm = new BooksViewModel() { Books = books };
            return View(vm);
        }

        public async Task<IActionResult> Details(Guid? id)
        {
            if (id is null) throw new ArgumentNullException(nameof(id));

            var book = await booksRepository.GetSelectedAsync((Guid)id);
            var vm = new BookDetailViewModel(book);

            return View(vm);
        }
    }
}
