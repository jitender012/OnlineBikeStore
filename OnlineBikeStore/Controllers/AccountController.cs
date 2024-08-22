﻿using AutoMapper;
using OnlineBikeStore.Extensions;
using OnlineBikeStore.Models;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;

namespace OnlineBikeStore.Controllers
{
    public class AccountController : Controller
    {
        BikeStoreEntities context = null;
        public AccountController()
        {
            context = new BikeStoreEntities();
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(AccountViewModel data)
        {
            if (ModelState.IsValid)
            {
                user customer = new user()
                {
                    first_name = data.first_name,
                    last_name = data.last_name,
                    email = data.email,
                    password = data.password
                };
                context.users.Add(customer);
                context.SaveChanges();

                int uid = customer.user_id;
                user_role role = new user_role()
                {
                    role = data.role,
                    user_id = customer.user_id
                };
                context.user_role.Add(role);
                context.SaveChanges();

                TempData["RedirectToLoginMsg"] = "Account created! Login to continue.";
                return RedirectToAction("Login", "Account");
            }
            return View(data);
        }

        [Authorize]
        public ActionResult CustomerProfile(int userId)
        {
            var customer = context.users.Where(u => u.user_id == userId).FirstOrDefault();
            ProfileViewModel profileViewModel = AutoMapper.Mapper.Map<ProfileViewModel>(customer);

            return View(profileViewModel);
        }

        [HttpPost]
        public ActionResult UpdateProfile(ProfileViewModel customer)
        {
            if (ModelState.IsValid)
            {
                var userId = context.GetUserId(User.Identity.Name);
                var user = context.users.Find(userId);
                if (user == null)
                {
                    return HttpNotFound();
                }
                else
                {
                    user.first_name = customer.first_name;
                    user.last_name = customer.last_name;                    
                    user.phone = customer.phone;
                    user.city = customer.city;
                    user.state = customer.state;
                    user.street = customer.street;
                    user.zip_code = customer.zip_code;

                    context.Entry(user).State = EntityState.Modified;
                    context.SaveChanges();

                    return RedirectToAction("UserProfile");
                }                
            }

            return View(customer);
        }

        public ActionResult Login()
        {
            if (TempData["RedirectToLoginMsg"] != null)
            {
                ViewBag.msg = TempData["RedirectToLoginMsg"];
            }
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel data)
        {
            if (ModelState.IsValid)
            {
                var user = context.users.FirstOrDefault(u => u.email == data.email && u.password == data.password);
                if (user != null)
                {
                    FormsAuthentication.SetAuthCookie(data.email, false);
                    TempData["SuccessMessage"] = "Login successfully!";

                    if (User.IsInRole("admin"))
                    {
                        return RedirectToAction("Dashboard", "Dashboard");
                    }

                    return RedirectToAction("Home", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Invalid username or password");
                    return View(data);
                }
            }
            else
            {
                ModelState.AddModelError("", "Invalid username or password");
                return View(data);
            }
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Home", "Home");
        }

        [Authorize]
        public ActionResult UserProfile()
        {
            if (User.Identity.IsAuthenticated)
            {
                var email = User.Identity.Name;
                var name = context.users.Where(e => e.email == email).Select(s => s.first_name).FirstOrDefault();
                return View("UserProfile", "~/Views/Shared/_CustomerLayout.cshtml", name);
            }
            else
                return RedirectToAction("Login");
        }
        [Authorize]
        public PartialViewResult UserInformation()
        {
            var uname = User.Identity.Name;
            var user = context.users.Where(u => u.email == uname).SingleOrDefault();

            ProfileViewModel profileView = Mapper.Map<ProfileViewModel>(context.users.Where(u => u.user_id == user.user_id).SingleOrDefault());
            return PartialView("_UserInformation", profileView);
        }                    

    }
}