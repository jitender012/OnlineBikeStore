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
            if (User.Identity.IsAuthenticated)
            {

                //get user id of logged in user
                var userId = context.users
                        .Where(x => x.email == User.Identity.Name)
                        .Select(u => u.user_id)
                        .SingleOrDefault();

                // Map the user to the UserViewModel
                var userVm = Mapper.Map<UserViewModel>(context.users
                    .Where(u => u.user_id == userId)
                    .SingleOrDefault());

                // Get the user's cart items
                var userCart = context.userCarts
                    .Where(c => c.user_id == userId)
                    .ToList();

                //get product id's from user cart
                var productIds = userCart.Select(q => q.product_id).ToList();

                // Fetch the products from the product IDs
                var cartProducts = context.products
                                          .Where(p => productIds.Contains(p.product_id))
                                          .ToList();

                // Map the products to the ProductViewModel
                var cartProductsVM = Mapper.Map<List<ProductViewModel>>(cartProducts);

                CartViewModel cart = new CartViewModel()
                {
                    user = userVm,
                    products = cartProductsVM
                };

                return View(cart);
            }
            return View("Error");
        }

        public ActionResult AddToCart(int pId)
        {
            if (User.Identity.IsAuthenticated)
            {
                var userId = context.users.Where(x => x.email == User.Identity.Name).Select(u => u.user_id).SingleOrDefault();
                if (pId > 0)
                {
                    userCart cart = new userCart
                    {
                        product_id = pId,
                        user_id = userId
                    };

                    context.userCarts.Add(cart);
                    context.SaveChanges();

                    return RedirectToAction("CartIndex", "Cart");
                }
                else
                {
                    return RedirectToAction("Error");
                }
            }
            else
            {
                TempData["RedirectToLoginMsg"] = "Login to add item into cart.";
                return RedirectToAction("Login","Account");
            }
          
        }
    }
}