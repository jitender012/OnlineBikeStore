using OnlineBikeStore.Models;
using System;
using System.Linq;
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


        [HttpGet]
        public JsonResult GetProductsNotInAnyStore()
        {
            var productsNotInAnyStore = context.products
                        .Where(p => !context.stocks
                        .Any(s => s.product_id == p.product_id))
                        .Select(x => new ProductViewModel
                        {
                            product_id = x.product_id,
                            product_name = x.product_name,
                            model_year = x.model_year
                        })
                        .ToList();

            return Json(productsNotInAnyStore, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AddOrUpdateStock(StockViewModel data)
        {
            try
            {
                // Check if the stock already exists for the given product and store
                var existingStock = context.stocks
                    .FirstOrDefault(s => s.product_id == data.product_id && s.store_id == data.store_id);

                if (existingStock != null)
                {
                    // Update the existing stock quantity
                    existingStock.quantity = data.quantity;
                }
                else
                {
                    // Add new stock
                    stock newStock = new stock
                    {
                        product_id = data.product_id,
                        store_id = data.store_id,
                        quantity = data.quantity
                    };
                    context.stocks.Add(newStock);
                }

                // Save changes to the database
                context.SaveChanges();
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult RemoveStockFromStore(int product_id, int store_id)
        {
            try
            {
                if (product_id < 1 || store_id < 1)
                {
                    return Json(new { success = false, message = "Invalid data" });
                }

                var stock = context.stocks
                    .Where(s => s.product_id == product_id && s.store_id == store_id)
                    .FirstOrDefault();

                if (stock == null)
                {
                    return Json(new { success = false, message = "Stock not found" });
                }

                context.stocks.Remove(stock);
                context.SaveChanges();
                return Json(new { success = true, message = "Item Reomved" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
    }
}
