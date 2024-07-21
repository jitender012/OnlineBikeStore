using AutoMapper;
using OnlineBikeStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public ActionResult Product(int pId)
        {
            var product = context.products.SingleOrDefault(c => c.product_id == pId);
            var category = context.categories.SingleOrDefault(x => x.category_id == product.category_id);
            var brand = context.brands.SingleOrDefault(x => x.brand_id == product.brand_id);
            var wishList = context.wishlists.FirstOrDefault(w => w.p_id == pId);
            // Check if the product found
            if (product == null)
            {
                return HttpNotFound("Product not found.");
            }

            bool isInCart = false;
            if (User.Identity.IsAuthenticated)
            {
                int uId = context.users.Where(u => u.email == User.Identity.Name).Select(q => q.user_id).SingleOrDefault();
                isInCart = context.userCarts.Any(c => c.product_id == pId && c.user_id ==uId);

            }
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
                isInCart = isInCart
            };

            return View(productDetailsViewModel);
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