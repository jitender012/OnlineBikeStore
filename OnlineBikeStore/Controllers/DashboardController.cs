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

            return View();
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
    }
}