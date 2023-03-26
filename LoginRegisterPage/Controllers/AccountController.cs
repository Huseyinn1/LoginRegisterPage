using LoginRegisterPage.Entities;
using LoginRegisterPage.Models;
using Microsoft.AspNetCore.Mvc;
using NETCore.Encrypt.Extensions;

namespace LoginRegisterPage.Controllers
{

    public class AccountController : Controller
    {
        private readonly DataBaseContext _databaseContext;
        private readonly IConfiguration _configuration;
        public AccountController(DataBaseContext databaseContext, IConfiguration configuration)
        {
            _databaseContext = databaseContext;
            _configuration = configuration;
        }

        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            
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
                if (ModelState.IsValid)
                {
                    string md5Salt = _configuration.GetValue<string>("AppSettings:MD5Salt");
                    string saltedPassword = model.Password + md5Salt;
                    string hashedPassword = saltedPassword.MD5();


                    User user = new()
                    {
                        UserName = model.UserName,
                        Password = hashedPassword
                    };
                    _databaseContext.Users.Add(user);
                    int affectedRowAccount = _databaseContext.SaveChanges();

                    if (affectedRowAccount == 0)
                    {
                        ModelState.AddModelError("", "user can not be  added.");
                    }
                    else
                    {
                        return RedirectToAction(nameof(Login));
                    }
                }

            }
            return View();
        }
        public IActionResult Profile()
        {
            return View();
        }


    }
}
