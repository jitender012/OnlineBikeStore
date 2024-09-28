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
     
        public ActionResult Contact()
        {
            return View();
        }
        public ActionResult About()
        {
            return View();
        }
        public ActionResult NotFound()
        {
            Response.StatusCode = 404;
            return View("Error");
        }
    }
}