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
        BikeStoreEntities context;
        public AccountController()
        {
            context = new BikeStoreEntities();
        }

        public ActionResult Create()
        {
            return View();
        }


        //Create new customer
        [HttpPost]
        public ActionResult Create(AccountViewModel data)
        {
            if (ModelState.IsValid)
            {
                // Check if the email already exists
                var existingUser = context.users.FirstOrDefault(u => u.email == data.email);

                if (existingUser != null)
                {
                    ModelState.AddModelError("email", "This email is already registered.");
                    return View(data);
                }
                user customer = new user()
                {
                    first_name = data.first_name,
                    last_name = data.last_name,
                    email = data.email,
                    password = Password.EncryptPassword(data.password)
                };
                context.users.Add(customer);
                context.SaveChanges();

                //Save user role and Set user role to "customer"
                int uid = customer.user_id;
                user_role role = new user_role()
                {
                    role = "customer",
                    user_id = customer.user_id
                };
                context.user_role.Add(role);
                context.SaveChanges();

                TempData["RedirectToLoginMsg"] = "Account created! Login to continue.";
                return RedirectToAction("Login", "Account");
            }
            return View(data);
        }

        [HttpPost]
        public ActionResult UpdateProfile(UserViewModel customer)
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

                    return RedirectToAction("UserAccount", new { linkId = 0 });
                }
            }

            return View(customer);
        }

        public ActionResult Login()
        {
            if (TempData["RedirectToLoginMsg"] != null)
            {
                ViewBag.SuccessMsg = TempData["RedirectToLoginMsg"];
            }
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel data)
        {
            if (ModelState.IsValid)
            {
                string hashedPassword = Password.EncryptPassword(data.password);
                var user = context.users.FirstOrDefault(u => u.email == data.email && u.password == hashedPassword);
                if (user != null)
                {
                    FormsAuthentication.SetAuthCookie(data.email, true);
                    TempData["SuccessMessage"] = "Login successfully!";

                    if (user.user_role.Select(x => x.role).SingleOrDefault() == "admin")
                    {
                        return RedirectToAction("Dashboard", "Dashboard");
                    }
                    else
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

        public ActionResult ResetPassword()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ResetPassword(LoginViewModel data)
        {
            if (ModelState.IsValid)
            {
                //Get user with email id 
                var user = context.users
                                        .Where(u => u.email == data.email)
                                        .SingleOrDefault();
                if (user==null)
                {
                    ModelState.AddModelError("email", "Email ID is not registered.");
                    return View(data);
                }
                string hashedPassword = Password.EncryptPassword(data.password);
                if (hashedPassword==user.password)
                {
                    ModelState.AddModelError("password", "Old password can't be used as new password.");
                    return View(data);
                }
                user.password = hashedPassword;
                context.SaveChanges();

                TempData["RedirectToLoginMsg"] = "Password reset successfully. Login to continue.";
                return RedirectToAction("Login", "Account");               
            }
            return View(data);
        }
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Home", "Home");
        }

        [Route("Account/Profile/")]
        [Authorize]
        public ActionResult UserAccount(int linkId)
        {
            ViewBag.LinkId = linkId;

            var email = User.Identity.Name;
            var name = context.users
                .Where(e => e.email == email)
                .Select(s => s.first_name)
                .FirstOrDefault();

            return View("UserAccount", "~/Views/Shared/_CustomerLayout.cshtml", name);

        }

        [Authorize]
        public PartialViewResult UserInformation()
        {
            var userId = context.GetUserId(User.Identity.Name);

            UserViewModel profileView = Mapper.Map<UserViewModel>(context.users.Where(u => u.user_id == userId).SingleOrDefault());
            return PartialView("_UserInformation", profileView);
        }

        [Authorize]
        public JsonResult GetUserId()
        {
            var userId = context.GetUserId(User.Identity.Name);
            return Json(userId, JsonRequestBehavior.AllowGet);
        }
    }

}