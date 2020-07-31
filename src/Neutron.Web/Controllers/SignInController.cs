using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Neutron.Infrastructure;
using Neutron.Web.ViewModels;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace Neutron.Web.Controllers
{
    [Route("[controller]")]
    public class SignInController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;

        public SignInController(SignInManager<ApplicationUser> signInManager)
        {
            _signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateSignInViewModel viewModel)
        {
            SignInResult result = await _signInManager.PasswordSignInAsync(viewModel.UserName, viewModel.Password, isPersistent: true, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Event");
            }

            TempData["Failure"] = "Wrong UserName or Password";

            return View(viewModel);
        }
    }
}
