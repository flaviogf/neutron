using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Neutron.Infrastructure;
using Neutron.Web.ViewModels;

namespace Neutron.Web.Controllers
{
    [Route("[controller]")]
    public class SignUpController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public SignUpController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateSignUpViewModel viewModel)
        {
            var user = new ApplicationUser
            {
                UserName = viewModel.UserName,
            };

            IdentityResult result = await _userManager.CreateAsync(user, viewModel.Password);

            if (!result.Succeeded)
            {
                TempData["Failure"] = result.Errors.Select(it => it.Description).FirstOrDefault();

                return View(viewModel);
            }

            await _signInManager.SignInAsync(user, isPersistent: true);

            TempData["Success"] = "Your account has been created successfully";

            return RedirectToAction("Index", "Event");
        }
    }
}
