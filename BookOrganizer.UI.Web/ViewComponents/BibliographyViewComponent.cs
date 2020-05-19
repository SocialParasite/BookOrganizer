using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace BookOrganizer.UI.Web.ViewComponents
{
    public sealed class BibliographyViewComponent : ViewComponent
    {

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View();
        }
    }
}
