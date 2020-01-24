using System;
using System.Threading.Tasks;
using BookOrganizer.DA;
using BookOrganizer.Domain;
using BookOrganizer.UI.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace BookOrganizer.UI.Web.Controllers
{
    public class PublisherController : Controller
    {
        private readonly IPublisherLookupDataService publisherLookupDataService;
        private readonly IRepository<Publisher> publishersRepository;

        public PublisherController(IPublisherLookupDataService publisherLookupDataService,
            IRepository<Publisher> publishersRepository)
        {
            this.publisherLookupDataService = publisherLookupDataService ?? throw new ArgumentNullException(nameof(publisherLookupDataService));
            this.publishersRepository = publishersRepository ?? throw new ArgumentNullException(nameof(publishersRepository));
        }

        public async Task<IActionResult> Index()
        {
            var publishers = await publisherLookupDataService.GetPublisherLookupAsync(nameof(PublishersViewModel));
            var vm = new PublishersViewModel() { Publishers = publishers };
            return View(vm);
        }

        public async Task<IActionResult> Details(Guid? Id)
        {
            if (Id == null) throw new ArgumentNullException(nameof(Id));

            var publisher = await publishersRepository.GetSelectedAsync((Guid)Id);
            var vm = new PublisherDetailViewModel(publisher);

            return View(vm);
        }

        public async Task<IActionResult> Edit(Guid? Id)
        {
            if (ModelState.IsValid)
            {
                if (Id == null) throw new ArgumentNullException(nameof(Id));

                var publisher = await publishersRepository.GetSelectedAsync((Guid)Id);

                var vm = new PublisherDetailViewModel(publisher);

                return View(vm);
            }

            return Redirect("/");
        }
    }
}
