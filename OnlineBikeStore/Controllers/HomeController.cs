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

        [Authorize(Roles ="admin, customer")]
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
        public ActionResult Product(int id)
        {
            var product = context.products.SingleOrDefault(c => c.product_id == id);
            var category = context.categories.SingleOrDefault(x => x.category_id == product.category_id);
            var brand = context.brands.SingleOrDefault(x => x.brand_id == product.brand_id);
            var wishList = context.wishlists.FirstOrDefault(w=>w.p_id==id);
            // Check if the product found
            if (product == null)
            {
                return HttpNotFound("Product not found.");
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
                brand_name = brand.brand_name
            };

            return View(productDetailsViewModel);          
        }
    }
}