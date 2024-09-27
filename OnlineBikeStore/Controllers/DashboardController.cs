using OnlineBikeStore.Models;
using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace OnlineBikeStore.Controllers
{
    [Authorize(Roles = "admin")]
    public class DashboardController : Controller
    {
        BikeStoreEntities context;
        public DashboardController()
        {
            context = new BikeStoreEntities();
        }
        // GET: Dashboard
        public ActionResult Dashboard()
        {
            if (TempData["SuccessMessage"] != null)
            {
                ViewBag.SuccessMessage = TempData["SuccessMessage"];
            }
            var orders = context.orders;
            var orderItmes = context.order_items;

            DashboardViewModel dashboardData = new DashboardViewModel()
            {
                TotalSales = orderItmes.Where(x=>x.order.order_status==2)
                                    .Sum(x => x.list_price),

                ItemsDelivered = orderItmes
                                    .Where(x => x.order.order_status == 2)
                                    .Count(),

                OrderCancelled = orders
                                    .Count(x => x.order_status == 3),

                TotalCustomers = context.user_role
                                    .Where(ur => ur.role == "customer")
                                    .Select(ur => ur.user_id)
                                    .Distinct()
                                    .Count(),               

                LowStocks = context.stocks
                                .Where(x => x.quantity <= 5)
                                .Join(context.products,
                                      stock => stock.product_id,
                                      product => product.product_id,
                                      (stock, product) => new StockViewModel
                                      {
                                          product_id = stock.product_id,
                                          product_name = product.product_name,
                                          quantity = stock.quantity,
                                          store_id = stock.store_id                                          
                                      })
                                .ToList(),

                PendingOrders = context.orders
                                    .Where(x => x.order_status == 0 || x.order_status == 1)
                                    .Select(x => new OrderViewModel
                                    {
                                        order_id = x.order_id,
                                        order_date = x.order_date,
                                        order_status = x.order_status,
                                        total_price = x.order_items.Sum(z => z.list_price)
                                    }).ToList()
            };
            return View(dashboardData);
        }
        public ActionResult StoreDashboard(int storeId)
        {
            if (storeId > 0)
            {
                var store = context.stores
                            .Include(x => x.stocks)
                            .Where(s => s.store_id == storeId)
                            .FirstOrDefault();

                StoreDashboardViewModel storeViewData = new StoreDashboardViewModel()
                {
                    store_id = storeId,
                    store_name = store.store_name,
                    stock = store.stocks
                            .Where(x => x.store_id == storeId)
                            .Select(p => new StockViewModel
                            {
                                product_id = p.product_id,
                                product_image = p.product.url,
                                quantity = p.quantity,
                                product_name = p.product.product_name,
                                model_year = p.product.model_year,
                                store_id = p.store_id
                            })
                            .ToList()
                };

                return View(storeViewData);
            }
            else
                return View("Error");
        }
        public PartialViewResult GetStockInStore(int storeId)
        {
            if (storeId > 0)
            {
                var store = context.stores
                            .Include(x => x.stocks)
                            .Where(s => s.store_id == storeId)
                            .FirstOrDefault();

                StoreDashboardViewModel storeViewData = new StoreDashboardViewModel()
                {
                    store_id = storeId,
                    store_name = store.store_name,
                    stock = store.stocks
                            .Where(x => x.store_id == storeId)
                            .Select(p => new StockViewModel
                            {
                                product_id = p.product_id,
                                product_image = p.product.url,
                                quantity = p.quantity,
                                product_name = p.product.product_name,
                                model_year = p.product.model_year,
                                store_id = p.store_id
                            })
                            .ToList()
                };

                return PartialView("_StoreDashboard", storeViewData);
            }
            return PartialView("Error loading stock items.", JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetTotalSalesByWeek()
        {
            var salesData = context.orders
                .Where(o => o.order_status == (byte)OrderStatus.Delivered)
                .GroupBy(o => new
                {
                    Week = System.Globalization.CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(o.order_date,
                                   System.Globalization.CalendarWeekRule.FirstDay, DayOfWeek.Monday),
                    Year = o.order_date.Year
                })
                .Select(g => new
                {
                    Week = g.Key.Week,
                    Year = g.Key.Year,
                    TotalSales = g.Sum(o => o.order_items.Sum(i => i.list_price * i.quantity))
                })
                .OrderBy(s => s.Year).ThenBy(s => s.Week)
                .ToList();

            return Json(salesData, JsonRequestBehavior.AllowGet);
        }

    }
}