using OnlineBikeStore.Extensions;
using OnlineBikeStore.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace OnlineBikeStore.Controllers
{
    [Authorize]
    public class WishListController : Controller
    {
        BikeStoreEntities context;
        public WishListController()
        {
            context = new BikeStoreEntities();
        }

        // GET: WishList
        public ActionResult WishlistIndex()
        {
            var userId = context.GetUserId(User.Identity.Name);

            var wishListVM = (from w in context.wishlists
                              join p in context.products on w.product_id equals p.product_id
                              where w.user_id == userId
                              select new WishListViewModel
                              {
                                  wl_item_id = w.wl_item_id,
                                  product_id = w.product_id,
                                  user_id = w.user_id,
                                  product_name = p.product_name,
                                  price = p.list_price,
                                  image_url = p.url
                              }).ToList();

            return View(wishListVM);
        }

        [HttpPost]
        public JsonResult AddRemoveWishlist(int pId)
        {
            var userId = context.users
                                .Where(x => x.email == User.Identity.Name)
                                .Select(z => z.user_id)
                                .FirstOrDefault();

            var existingItem = context.wishlists
                                      .FirstOrDefault(w => w.product_id == pId && w.user_id == userId);

            // Remove item if already in wishlist
            if (existingItem != null)
            {
                context.wishlists.Remove(existingItem);
                context.SaveChanges();

                return Json(new { success = true, inWishlist = false });
            }
            // Add item if already not in the wishlist
            else
            {
                wishlist newItem = new wishlist
                {
                    product_id = pId,
                    user_id = userId
                };
                context.wishlists.Add(newItem);
                context.SaveChanges();

                return Json(new { success = true, inWishlist = true });
            }
        }

    }
}