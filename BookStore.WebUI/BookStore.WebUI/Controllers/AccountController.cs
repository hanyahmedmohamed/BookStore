﻿using BookStore.WebUI.Infrastructure.Abstract;
using BookStore.WebUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BookStore.WebUI.Controllers
{
    public class AccountController : Controller
    {
        IAuthProvider authProvider;
        public AccountController(IAuthProvider auth)
        {
            authProvider = auth;
        }
        public ViewResult Login()
        {
            return View();
        }
        [HttpPost]//hab3t hena eluser name w el password
        public ActionResult Login(LoginViewModel model,string returnUrl)
        {
            //
            if(ModelState.IsValid)
            {
                if (authProvider.Authenticate(model.Username, model.Password))
                    return Redirect(returnUrl??Url.Action("Index","Admin"));
                else
                {
                    ModelState.AddModelError("", "Incorrect Username/password");
                    return View();
                }
            }else
                return View();
        }
	}
}