using AutoMapper;
using LoginRegisterPage.Entities;
using LoginRegisterPage.Models;
using Microsoft.AspNetCore.Mvc;

namespace LoginRegisterPage.Controllers
{
    public class UserController : Controller
    {
        private readonly DataBaseContext _dataBaseContext;
        private readonly IMapper _mapper;



        public UserController(DataBaseContext dataBaseContext, IMapper mapper)
        {
            _dataBaseContext = dataBaseContext;
            _mapper = mapper;
        }

        public IActionResult Index()
        {
           List<UserModel> users = _dataBaseContext.Users.ToList().Select(x => _mapper.Map<UserModel>(x)).ToList();

            return View(users);
        }
    }
}
