
using AutoMapper;
using OnlineBikeStore.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace OnlineBikeStore.Controllers
{
    [Authorize(Roles = "admin")]
    public class StoreController : Controller
    {
        readonly BikeStoreEntities context;
        public StoreController()
        {
            context = new BikeStoreEntities();
        }
        // GET: Stores
        public ActionResult Index()
        {
            var stores = context.stores.ToList();
            List<StoreViewModel> storeView = Mapper.Map<List<StoreViewModel>>(stores);
            return View(storeView);
        }

        // GET: Stores
        public ActionResult Details(int id)
        {
            //Find store in database
            var store = context.stores.SingleOrDefault(c => c.store_id == id);
       
            // Check if the store found
            if (store == null)
            {
                return HttpNotFound("Store not found.");
            }

            //Map database store to view model
            var storeViewModel = Mapper.Map<StoreViewModel>(store);

            return View(storeViewModel);
        }
        
        // GET: Stores/Create
        public ActionResult Create()
        {         
            return View();
        }

        // POST: Stores/Create
        [HttpPost]
        public ActionResult Create(StoreViewModel data)
        {

            if (ModelState.IsValid)
            {
                try
                {                   
                    var store = Mapper.Map<store>(data);
                    context.stores.Add(store);
                    context.SaveChanges();
                    TempData["SuccessMessage"] = "Store created successfully!";
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {

                    ModelState.AddModelError(ex.Message, "An error occurred while creating the store. Please try again.");
                }
            }           
       
            return View(data);
        }
        // GET: Stores/Edit
        public ActionResult Edit(int? id)
        {          
            if (id > 0)
            {
                var store = context.stores.SingleOrDefault(c => c.store_id == id);

                if (store != null)
                {
                    StoreViewModel prodvm = Mapper.Map<StoreViewModel>(store);
                    return View(prodvm);
                }
                TempData["NotFound"] = "Store not found!";
                return View("Error");
            }
            return View("Error");
        }

        // POST: Stores/Edit
        [HttpPost]
        public ActionResult Edit(StoreViewModel data)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var storeEntity = context.stores.Find(data.store_id);
                    if (storeEntity == null)
                    {
                        return HttpNotFound();
                    }                    
                    storeEntity.store_name = data.store_name;

                    context.Entry(storeEntity).State = EntityState.Modified;
                    context.SaveChanges();

                    TempData["SuccessMessage"] = "Store updated successfully!";
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {

                    ModelState.AddModelError(ex.Message, "An error occurred while updating the store. Please try again.");
                }
            }          
            return View(data);
        }



        [HttpPost]
        public JsonResult Delete(int id)
        {
            if (id > 0)
            {
                try
                {
                    var store = context.stores.Find(id);
                    if (store == null)
                    {
                        return Json(new { success = false, message = "Store not found." });
                    }

                    context.stores.Remove(store);
                    context.SaveChanges();

                    return Json(new { success = true, message = "Store deleted successfully!" });
                }
                catch (Exception)
                {
                    // Log the exception here for further analysis if needed
                    return Json(new { success = false, message = "An error occurred while deleting the store. Please try again." });
                }
            }

            return Json(new { success = false, message = "Invalid store ID." });
        }

        public JsonResult GetStores()
        {
            var stores = context.stores.ToList();
            
            return Json(Mapper.Map<List<StoreViewModel>>(stores), JsonRequestBehavior.AllowGet);
        }
    }
}
