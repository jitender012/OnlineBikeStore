using AutoMapper;
using OnlineBikeStore.Extensions;
using OnlineBikeStore.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
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
        [Route("")]
        [Route("Home")]
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
            if (pId <= 0 && pId == null)
            {
                return View("Error");
            }

            // Check if the product found
            var product = context.products
                .SingleOrDefault(c => c.product_id == pId);
            if (product == null)
            {
                return HttpNotFound("Product not found.");
            }

            var categories = context.categories.ToList();
            ViewBag.Categories = Mapper.Map<List<CategoryViewModel>>(categories);


            var category = context.categories
                .SingleOrDefault(x => x.category_id == product.category_id);
            var brand = context.brands
                .SingleOrDefault(x => x.brand_id == product.brand_id);

            bool isInCart = false;
            bool isInWishlist = false;
            bool isPurchased = false;
            bool isReviewed = false;
            ViewBag.UserId = 0;


            if (User.Identity.IsAuthenticated)
            {
                int uId = context.GetUserId(User.Identity.Name);

                var userData = context.users
                       .Include(u => u.userCarts)
                       .Include(u => u.wishlists)
                       .Include(u => u.orders.Select(o => o.order_items))
                       .Include(u => u.feedbacks)
                       .SingleOrDefault(u => u.user_id == uId);

                if (userData != null)
                {
                    isInCart = userData.userCarts
                        .Any(c => c.product_id == pId);
                    isInWishlist = userData.wishlists
                        .Any(w => w.product_id == pId);
                    isPurchased = userData.orders
                        .Any(o => o.order_status == 2 && o.order_items.Any(oi => oi.product_id == pId));
                    isReviewed = userData.feedbacks
                        .Any(f => f.product_id == pId);

                    ViewBag.UserId = uId;
                }
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
                                                .Select(z => z.first_name + " " + z.last_name)
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
                Feedback = feedbackViewModel,
                currentStock = context.stocks
                    .Where(p => p.product_id == pId)
                    .Select(s => (int)s.quantity)
                    .DefaultIfEmpty(-1)
                    .FirstOrDefault()
            };

            return View(productDetailsViewModel);
        }
        public ActionResult Search(string searchWord)
        {
            var categories = context.categories.ToList();
            ViewBag.Categories = Mapper.Map<List<CategoryViewModel>>(categories);
            ViewBag.Brands = context.brands.Select(b => b.brand_name).ToList();


            if (searchWord != null)
            {
                ViewBag.searchString = searchWord;
                searchWord.ToLower();

                var products = context.products
                    .Where(p => p.product_name.Contains(searchWord) || p.description.Contains(searchWord) || p.brand.brand_name.Contains(searchWord) || p.category.category_name.Contains(searchWord))
                    .ToList();
                if (products != null)
                {
                    var productsVM = Mapper.Map<List<ProductViewModel>>(products);
                    return View(productsVM);
                }
            }
            var result = Mapper.Map<List<ProductViewModel>>(context.products.ToList());


            return View(result);
        }

        public ActionResult Filter(int catId)
        {
            var categories = context.categories.ToList();
            ViewBag.Categories = Mapper.Map<List<CategoryViewModel>>(categories);
            ViewBag.Brands = context.brands.Select(b => b.brand_name).ToList();
            var products = context.products.Where(p => p.category_id == catId).ToList();

            ViewBag.catId = catId;
            if (products != null)
            {
                var productsVM = Mapper.Map<List<ProductViewModel>>(products);
                return View("Search", productsVM);
            }
            return View("Error");
        }

        [HttpPost]
        public PartialViewResult SearchPartial(FilterDataModel filters, string searchString, string catId)
        {
            List<int> productIds;
            List<product> filteredItems = new List<product>();

            if (searchString != "")
            {
                searchString.ToLower();

                productIds = context.products
                    .Where(p => p.product_name.Contains(searchString) ||
                                p.description.Contains(searchString) ||
                                p.brand.brand_name.Contains(searchString) ||
                                p.category.category_name.Contains(searchString))
                    .Select(o => o.product_id)
                    .ToList();
            }
            else
            {
                productIds = context.products
                    .Where(p => p.category_id.ToString() == catId)
                    .Select(x => x.product_id)
                    .ToList();
            }

            if (filters.brands != null)
            {
                foreach (var brand in filters.brands)
                {
                    var Items = context.products
                        .Where(p => productIds.Contains(p.product_id) && p.brand.brand_name == brand)
                        .ToList();
                    filteredItems.AddRange(Items);
                }
            }

            else
            {
                filteredItems = context.products
                    .Where(p => productIds.Contains(p.product_id))
                    .ToList();
            }

            if (filteredItems != null)
            {
                var productsVM = Mapper.Map<List<ProductViewModel>>(filteredItems);
                return PartialView("_SearchPartial", productsVM);
            }
            return PartialView();
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

        public ActionResult NotFound()
        {
            Response.StatusCode = 404;
            return View("Error");
        }
    }
}