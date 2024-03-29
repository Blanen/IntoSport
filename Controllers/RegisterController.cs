﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IntoSport.Models;

namespace IntoSport.Controllers
{
    public class RegisterController : MainController
    {
        public ActionResult Index()
        {
            if(Request.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        public ActionResult Succes()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(Register u)
        {
            if (ModelState.IsValid)
            {
                if (!u.exists())
                {
                    u.createAccount();
                    return Redirect("register/Succes");
                }
                else
                {
                    ModelState.AddModelError("emailError", "Email is al in gebruik");
                }
                

            }
            return View("index", u);
        }

        

    }
}