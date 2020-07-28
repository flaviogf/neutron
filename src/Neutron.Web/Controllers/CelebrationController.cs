using Microsoft.AspNetCore.Mvc;
using Neutron.Web.ViewModels;

namespace Neutron.Web.Controllers
{
    [Route("[controller]")]
    public class CelebrationController : Controller
    {
        [HttpGet("Create")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CreateCelebrationViewModel viewModel)
        {
            return RedirectToAction("Create", "Celebration");
        }
    }
}
