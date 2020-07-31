using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Neutron.Infrastructure;

namespace Neutron.Web.Controllers
{
    [Route("[controller]")]
    public class SignOutController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;

        public SignOutController(SignInManager<ApplicationUser> signInManager)
        {
            _signInManager = signInManager;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Destroy()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction("Create", "SignIn");
        }
    }
}
