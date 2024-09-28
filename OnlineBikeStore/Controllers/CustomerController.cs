using AutoMapper;
using OnlineBikeStore.Extensions;
using OnlineBikeStore.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineBikeStore.Controllers
{
    public class CustomerController : Controller
    {
         BikeStoreEntities context;
        public CustomerController()
        {
            context = new BikeStoreEntities();
        }
        // GET: Customer
        [Authorize(Roles ="admin")]
        public ActionResult Index()
        {
            IEnumerable<user> user = context.users;
            var userVM = AutoMapper.Mapper.Map<IEnumerable<UserViewModel>>(user);
            return View(userVM);
        }
        public ActionResult Details(int? userId)
        {
            if (userId < 0 && userId == null)
            {
                return View("Error");
            }
            user user = context.users
                .Where(u => u.user_id == userId)
                .SingleOrDefault();
            if (user == null)
            {
                ViewBag.ErrorMsg = "Customer not existed.";
                return View("Error");
            }

            UserViewModel userVM = Mapper.Map<UserViewModel>(user);
            return View(userVM);
        }

        public ActionResult Edit(int? userId)
        {
            if (userId<0 && userId==null)
            {
                return View("Error");
            }
            user user = context.users
                .Where(u => u.user_id == userId)
                .SingleOrDefault();
            if (user==null)
            {
                ViewBag.ErrorMsg = "Customer not existed.";
                return View("Error");
            }

            UserViewModel userVM = Mapper.Map<UserViewModel>(user);
            return View(userVM);
        }

        [HttpPost]
        public ActionResult Edit(UserViewModel customer)
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

                    return RedirectToAction("Index");
                }
            }

            return View(customer);
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

                    return RedirectToAction("UserAccount","Account", new { linkId = 0 });
                }
            }

            return View(customer);
        }
        [Authorize]
        public PartialViewResult UserInformation()
        {
            var userId = context.GetUserId(User.Identity.Name);

            UserViewModel profileView = Mapper.Map<UserViewModel>(context.users.Where(u => u.user_id == userId).SingleOrDefault());
            return PartialView("_UserInformation", profileView);
        }
    }
}