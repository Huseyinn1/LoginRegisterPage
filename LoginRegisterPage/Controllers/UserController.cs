﻿using AutoMapper;
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
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(CreateUserModel model)
        {
            if (ModelState.IsValid)
            {
                if (_dataBaseContext.Users.Any(x => x.UserName.ToLower() == model.UserName.ToLower()))
                {
                    ModelState.AddModelError(nameof(model.UserName), "Username already exists.");
                    return View(model);
                }
                User user = _mapper.Map<User>(model);
                _dataBaseContext.Users.Add(user);
                _dataBaseContext.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View();
        }
        public IActionResult Edit(Guid id)

        {
            User user = _dataBaseContext.Users.Find(id);
            EditUserModel model = _mapper.Map<EditUserModel>(user);
            return View(model);
        }
        [HttpPost]
        public IActionResult Edit(Guid id, EditUserModel model)
        {
            if (ModelState.IsValid)
            {
                if (_dataBaseContext.Users.Any(x => x.UserName.ToLower() == model.UserName.ToLower() && x.Id != id))
                {
                    ModelState.AddModelError(nameof(model.UserName), "Username already exists.");
                    return View(model);
                }
                User user = _dataBaseContext.Users.Find(id);
                _mapper.Map(model, user);
                _dataBaseContext.SaveChanges();
                return RedirectToAction(nameof(Index));
            }



            return View();
        }
        
        public IActionResult Delete(Guid id)
        {

            User user = _dataBaseContext.Users.Find(id);
            if (user != null)
            { 
                _dataBaseContext.Users.Remove(user);


                _dataBaseContext.SaveChanges();
            }
            return RedirectToAction(nameof(Index));




            return View();
        }

    }
}
