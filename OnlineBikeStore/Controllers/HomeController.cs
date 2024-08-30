using AutoMapper;
using OnlineBikeStore.Extensions;
using OnlineBikeStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;

namespace OnlineBikeStore.Controllers
{
    public class HomeController : Controller
    {
        BikeStoreEntities context;
        public HomeController()
        {
            context = new BikeStoreEntities();
        }
        public ActionResult Test()
        {
            return View();
        }
        public ActionResult Index()
        {
            var categories = context.categories.ToList();
            ViewBag.Categories = Mapper.Map<List<CategoryViewModel>>(categories);
            return View();
        }

        public ActionResult Home()
        {
            if (TempData["SuccessMessage"] != null)
            {
                ViewBag.SuccessMessage = TempData["SuccessMessage"];
            }

            var categories = context.categories.ToList();
            ViewBag.Categories = Mapper.Map<List<CategoryViewModel>>(categories);
            var brands = context.brands.ToList();
            ViewBag.Brands = Mapper.Map<List<BrandViewModel>>(brands);

            var products = context.products.Select(x => new ProductDetailsViewModel
            {
                product_id = x.product_id,
                product_name = x.product_name,
                description = x.description,
                product_type = x.product_type,
                list_price = x.list_price,
                category_id = x.category_id,
                category_name = x.category.category_name,
                brand_id = x.brand_id,
                brand_name = x.brand.brand_name,
                model_year = x.model_year,
                url = x.url
            }).ToList();
            return View(products);
        }


        [Authorize(Roles = "admin, customer")]
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult Product(int? pId)
        {
            if (pId > 0 && pId != null)
            {
                var categories = context.categories.ToList();
                ViewBag.Categories = Mapper.Map<List<CategoryViewModel>>(categories);

                var product = context.products
                    .SingleOrDefault(c => c.product_id == pId);
                var category = context.categories
                    .SingleOrDefault(x => x.category_id == product.category_id);
                var brand = context.brands
                    .SingleOrDefault(x => x.brand_id == product.brand_id);

                // Check if the product found
                if (product == null)
                {
                    return HttpNotFound("Product not found.");
                }
                

                bool isInCart = false;
                bool isInWishlist = false;
                bool isPurchased = false;
                bool isReviewed = false;
                ViewBag.UserId = 0;
                if (User.Identity.IsAuthenticated)               {
                    int uId = context.GetUserId(User.Identity.Name);

                    isInCart = context.userCarts
                        .Any(c => c.product_id == pId && c.user_id == uId);

                    isInWishlist = context.wishlists
                        .Any(c => c.product_id == pId && c.user_id == uId);

                    isPurchased = context.orders
                                            .Where(x => x.user.user_id == uId && x.order_items.Any(z => z.product_id == pId))
                                            .Any();
                    isReviewed = context.feedbacks
                        .Any(x=>x.product_id == pId && x.customer_id == uId);

                    ViewBag.UserId = uId;
                }

                var feedbackViewModel = context.feedbacks
                                        .Where(x => x.product_id == pId)
                                        .Select(f => new FeedbackViewModel
                                        {
                                            feedback_id = f.feedback_id,
                                            ratingValue = f.ratingValue,
                                            customer_id = f.customer_id,
                                            date = f.date,
                                            feedback_text = f.feedback_text,
                                            image_url = f.image_url,
                                            customer_name = context.users
                                                    .Where(x => x.user_id == f.customer_id)
                                                    .Select(z => z.first_name +" "+ z.last_name)
                                                    .FirstOrDefault()                                            
                                        })
                                        .ToList();

                //Map database product to view model
                ProductDetailsViewModel productDetailsViewModel = new ProductDetailsViewModel()
                {
                    product_name = product.product_name,
                    product_id = product.product_id,
                    list_price = product.list_price,
                    product_type = product.product_type,
                    description = product.description,
                    url = product.url,
                    model_year = product.model_year,
                    category_id = product.category_id,
                    brand_id = product.brand_id,
                    category_name = category.category_name,
                    brand_name = brand.brand_name,
                    isInCart = isInCart,
                    isInWishList = isInWishlist,
                    isPurchased = isPurchased,
                    isReviewed = isReviewed,
                    Feedback = feedbackViewModel
                };

                return View(productDetailsViewModel);
            }
            else
                return View("Error");
        }
        public ActionResult Search(string searchWord)
        {
            var categories = context.categories.ToList();
            ViewBag.Categories = Mapper.Map<List<CategoryViewModel>>(categories);

            if (searchWord != null)
            {
                searchWord.ToLower();

                var products = context.products.Where(p => p.product_name.Contains(searchWord) || p.description.Contains(searchWord) || p.brand.brand_name.Contains(searchWord) || p.category.category_name.Contains(searchWord)).ToList();
                if (products != null)
                {
                    var productsVM = Mapper.Map<List<ProductViewModel>>(products);
                    return View(productsVM);
                }
            }
            var result = Mapper.Map<List<ProductViewModel>>(context.products.ToList());
            return View(result);
        }

        //Search Products By Category
        public ActionResult FilterByCategory(int? categoryId)
        {
            var categories = context.categories.ToList();
            ViewBag.Categories = Mapper.Map<List<CategoryViewModel>>(categories);

            if (categoryId.HasValue && categoryId > 0)
            {
                var products = context.products.Where(p => p.category_id == categoryId).ToList();
                var productsVM = Mapper.Map<List<ProductViewModel>>(products);
                return View("Search", productsVM);
            }
            else return View("Search");
        }
    }
}