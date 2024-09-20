using AutoMapper;
using OnlineBikeStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
namespace OnlineBikeStore.Controllers
{
    public class StockController : Controller
    {
        BikeStoreEntities context;
        public StockController()
        {
            context = new BikeStoreEntities();
        }
        // GET: Stock
        public ActionResult Index()
        {
            return View();
        }
       

        [HttpGet]
        public JsonResult GetProductsNotInAnyStore()
        {
            var productsNotInAnyStore = context.products
                        .Where(p => !context.stocks
                        .Any(s => s.product_id == p.product_id))
                        .Select(x=>new ProductViewModel
                        {
                            product_id = x.product_id,
                            product_name = x.product_name,
                            model_year = x.model_year
                        })
                        .ToList();          

            return Json(productsNotInAnyStore, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AddStock(StockViewModel data)
        {
            try
            {
                stock s = new stock();
                s.product_id = data.product_id;
                s.store_id = data.store_id;
                s.quantity = data.quantity;
                var newStock = context.stocks.Add(s);
                context.SaveChanges();
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
        [HttpPost]
        public JsonResult RemoveStock(StockViewModel data)
        {
            try
            {
                var stock = context.stocks.Where(s => s.product_id == data.product_id && s.store_id == data.store_id).FirstOrDefault();

                context.stocks.Remove(stock);
                context.SaveChanges();
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
    }
}
