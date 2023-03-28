using LoginRegisterPage.Entities;
using LoginRegisterPage.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NETCore.Encrypt.Extensions;
using System.Security.Claims;

namespace LoginRegisterPage.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly DataBaseContext _databaseContext;
        private readonly IConfiguration _configuration;
        public AccountController(DataBaseContext databaseContext, IConfiguration configuration)
        {
            _databaseContext = databaseContext;
            _configuration = configuration;
        }

        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                string md5Salt = _configuration.GetValue<string>("AppSettings:MD5Salt");
                string saltedPassword = model.Password + md5Salt;
                string hashedPassword = saltedPassword.MD5();

                User user = _databaseContext.Users.SingleOrDefault(x=>x.UserName.ToLower()==model.UserName.ToLower()
                && x.Password == hashedPassword );
                
                if (user != null)
                {
                    if (user.Locked)
                    {
                        ModelState.AddModelError(nameof(model.UserName), "User is locked");
                        return View(model);
                    }

                    List<Claim> claims = new List<Claim>();
                    claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));
                    claims.Add(new Claim(ClaimTypes.Name,user.NameSurname ?? String.Empty));
                    claims.Add(new Claim("Username",user.UserName));

                    ClaimsIdentity identity = new ClaimsIdentity(claims,
                        CookieAuthenticationDefaults.AuthenticationScheme);

                    ClaimsPrincipal principal = new ClaimsPrincipal(identity);
                    
                    HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                    return RedirectToAction("index", "home");

                }
                else
                {
                    ModelState.AddModelError("", "Username or password is incorrect");
                }
            }

            return View(model);
        }
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        public IActionResult Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                if(_databaseContext.Users.Any(x=>x.UserName.ToLower() == model.UserName.ToLower()))
                {
                    ModelState.AddModelError(nameof(model.UserName), "Username already exists");
                    return View(model);
                }


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


            return View();
        }
        public IActionResult Profile()
        {
            return View();
        }
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction(nameof(Login));
            
        }

    }
}
