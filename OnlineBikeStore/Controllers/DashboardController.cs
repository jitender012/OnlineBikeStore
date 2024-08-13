using AutoMapper;
using OnlineBikeStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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
        public ActionResult StoreDashboard(int store_id)
        {
            var store = context.stores.Where(s => s.store_id == store_id).SingleOrDefault();
            return View(Mapper.Map<StoreViewModel>(store));
        }
    }
}