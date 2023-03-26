using LoginRegisterPage.Models;
using Microsoft.AspNetCore.Mvc;

namespace LoginRegisterPage.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                //login işlemleri
            }
            return View(model);
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {

            }
            return View();
        }
        public IActionResult Profile()
        {
            return View();
        }


    }
}
