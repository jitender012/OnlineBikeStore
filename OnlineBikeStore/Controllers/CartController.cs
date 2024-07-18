using AutoMapper;
using OnlineBikeStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineBikeStore.Controllers
{
    public class CartController : Controller
    {
        BikeStoreEntities context;
        public CartController()
        {
            context = new BikeStoreEntities();
        }
        // GET: Cart
        public ActionResult CartIndex()
        {
            var userId = context.users.Where(x => x.email == User.Identity.Name).Select(u=>u.user_id).SingleOrDefault();
            var userCart = context.userCarts.Where(c=>c.user_id==userId).ToList();

           
            return View(userCart);           
        }

        public ActionResult AddToCart(int pid)
        {
            var userId = context.users.Where(x => x.email == User.Identity.Name).Select(u => u.user_id).SingleOrDefault();

            if (pid > 0)
            {
                CartViewModel cart = new CartViewModel()
                {
                    product_id = pid,
                    user_id = userId,
                };

                var result = context.userCarts.Add(Mapper.Map<userCart>(cart));
                return RedirectToAction("Index", "Cart");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }
    }
}