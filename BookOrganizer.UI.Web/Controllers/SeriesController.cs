using System;
using System.Threading.Tasks;
using BookOrganizer.DA;
using BookOrganizer.Domain;
using BookOrganizer.UI.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace BookOrganizer.UI.Web.Controllers
{
    public class SeriesController : Controller
    {
        private readonly ISeriesLookupDataService seriesLookupDataService;
        private readonly IRepository<Series> seriesRepository;

        public SeriesController(ISeriesLookupDataService seriesLookupDataService,
            IRepository<Series> seriesRepository)
        {
            this.seriesLookupDataService = seriesLookupDataService ?? throw new ArgumentNullException(nameof(seriesLookupDataService));
            this.seriesRepository = seriesRepository ?? throw new ArgumentNullException(nameof(SeriesController.seriesRepository));
        }

        public async Task<IActionResult> Index()
        {
            var series = await seriesLookupDataService.GetSeriesLookupAsync(nameof(SeriesViewModel));
            var vm = new SeriesViewModel() { Series = series };
            return View(vm);
        }

        public async Task<IActionResult> Details(Guid? Id)
        {
            if (Id == null) throw new ArgumentNullException(nameof(Id));

            var series = await seriesRepository.GetSelectedAsync((Guid)Id);
            var vm = new SeriesDetailViewModel(series);

            return View(vm);
        }

        public async Task<IActionResult> Edit(Guid? Id)
        {
            if (ModelState.IsValid)
            {
                if (Id == null) throw new ArgumentNullException(nameof(Id));

                var series = await seriesRepository.GetSelectedAsync((Guid)Id);

                var vm = new SeriesDetailViewModel(series);

                return View(vm);
            }

            return Redirect("/");
        }
    }
}
