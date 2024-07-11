using AutoMapper;
using OnlineBikeStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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
        public JsonResult GetStockInStore(int storeId)
        {
            if (storeId > 0)
            {
                List<InventoryDomainModel> inventories = new List<InventoryDomainModel>();
                var stocks = context.stocks.Where(s => s.store_id == storeId);
                foreach (var item in stocks)
                {
                    var storeProduct = context.products.Where(p => p.product_id == item.product_id).SingleOrDefault();
                    InventoryDomainModel inventory = new InventoryDomainModel();
                    inventory.store_id = storeId;
                    inventory.product_id = item.product_id;
                    inventory.product_name = storeProduct.product_name;
                    inventory.model_year = storeProduct.model_year;
                    if (item.quantity < 0 || item.quantity == null)
                    {
                        inventory.quantity = 0;
                    }
                    else
                    {
                        inventory.quantity = item.quantity.Value;
                    }

                    inventories.Add(inventory);
                }
                return Json(inventories, JsonRequestBehavior.AllowGet);
            }
            return Json("Error loading stock items.", JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult AddStock(int storeId)
        {

            var productIdsInStore = context.stocks.Where(s => s.store_id == storeId).Select(s => s.product_id).ToList();

            var productsNotInStore = context.products.Where(p => !productIdsInStore.Contains(p.product_id))
                .ToList();

            // Map the products to a view model if necessary
            var productViewModels = productsNotInStore.Select(p => new ProductViewModel
            {
                product_id = p.product_id,
                product_name = p.product_name,
                model_year = p.model_year
            }).ToList();

            return Json(productViewModels, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AddStock(StockViewModel data)
        {
            try
            {
                var newStock = context.stocks.Add(Mapper.Map<stock>(data));

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                // Log the exception and return a failure response
                return Json(new { success = false, message = ex.Message });
            }
        }

    }
}
