using AutoMapper;
using OnlineBikeStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WebGrease.Css.Extensions;

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
            AccountViewModel account = new AccountViewModel()
            {
                role = "customer"
            };
            return View(account);
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
                    password = data.password,
                };
                context.users.Add(customer);
                int uid = customer.user_id;
                user_role role = new user_role()
                {
                    role = data.role,
                    user_id = customer.user_id
                };
                context.user_role.Add(role);
            }

            return View();
        }
        [Authorize]
        public ActionResult CustomerProfile(int userId)
        {
            var customer = context.users.Where(u => u.user_id == userId);
            return View(AutoMapper.Mapper.Map<ProfileViewModel>(customer));
        }

        //public string GetCustomerName(string email)
        //{
        //    var customer_details = repository.getCustomerName(email);
        //    return customer_details;
        //}

        //[HttpPost]
        //public ActionResult CustomerProfile(AccountViewModel customer)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var customer_details = repository.UpdateCustomerDetails(customer.customer_id, customer);

        //        return View(customer_details);
        //    }

        //    return View();
        //}
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
                    TempData["SuccessMessage"] = "Login successfully";

                    if (User.IsInRole("admin"))
                    {
                        return RedirectToAction("Index", "Dashboard");
                    }

                    return RedirectToAction("Index", "Home");
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
        [Authorize]
        public PartialViewResult Feedback()
        {
            var userName = User.Identity.Name;
            var userId = context.users.Where(u => u.email == userName).Select(i => i.user_id).SingleOrDefault();
            var feedbacks = context.feedbacks.Where(f => f.customer_id == userId).ToList();
            return PartialView("_UserReviews");
        }
        [Authorize]
        public PartialViewResult UserOrders()
        {
            var uname = User.Identity.Name;
            var user = context.users.Where(u => u.email == uname).SingleOrDefault();

            ProfileViewModel profileView = Mapper.Map<ProfileViewModel>(context.users.Where(u => u.user_id == user.user_id).SingleOrDefault());
            return PartialView("_UserInformation", profileView);
        }

    }
}