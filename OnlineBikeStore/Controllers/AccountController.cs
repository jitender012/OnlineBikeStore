using OnlineBikeStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel data)
        {

            if (ModelState.IsValid)
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
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Home", "Home");
        }


    }
}