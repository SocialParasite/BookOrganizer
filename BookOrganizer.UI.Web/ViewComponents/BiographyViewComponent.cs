using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace BookOrganizer.UI.Web.ViewComponents
{
    public sealed class BiographyViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View();
        }
    }
}
