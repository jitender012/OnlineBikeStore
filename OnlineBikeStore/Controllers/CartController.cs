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
        [Authorize]
        public ActionResult CartIndex()
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
                return RedirectToAction("Login", "Account");
            }

        }

        [HttpPost]
        public JsonResult RemoveFromCart(int productId)
        {
            var userId = context.users.Where(x => x.email == User.Identity.Name).Select(u => u.user_id).SingleOrDefault();
            if (userId == 0)
            {
                return Json(new { success = false, message = "User not found." });
            }

            var cartItem = context.userCarts.FirstOrDefault(c => c.user_id == userId && c.product_id == productId);
            if (cartItem != null)
            {
                context.userCarts.Remove(cartItem);
                context.SaveChanges();
                return Json(new { success = true });
            }
            else
            {
                return Json(new { success = false, message = "Item not found in cart." });
            }
        }
    
    }
}