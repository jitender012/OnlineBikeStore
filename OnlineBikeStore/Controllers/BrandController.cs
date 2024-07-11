using AutoMapper;
using OnlineBikeStore.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineBikeStore.Controllers
{
    public class BrandController : Controller
    {
        BikeStoreEntities context;
        public BrandController()
        {
            context = new BikeStoreEntities();
        }
        // GET: brands
        public ActionResult Index()
        {
            var brands = context.brands.ToList();
            List<BrandViewModel> brandView = Mapper.Map<List<BrandViewModel>>(brands);
            return View(brandView);
        }

        // GET: brands
        public ActionResult Details(int brandId)
        {
            //Find brand in database
            var brand = context.brands.SingleOrDefault(c => c.brand_id == brandId);

            // Check if the brand found
            if (brand == null)
            {
                return HttpNotFound("brand not found.");
            }

            //Map database brand to view model
            var brandViewModel = Mapper.Map<BrandViewModel>(brand);

            return View(brandViewModel);
        }

        // GET: brands/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: brands/Create
        [HttpPost]
        public ActionResult Create(BrandViewModel data)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (data.ImageFile != null && data.ImageFile.ContentLength > 0)
                    {
                        // Save  image to the server
                        string fileName = Path.GetFileNameWithoutExtension(data.ImageFile.FileName);
                        string extension = Path.GetExtension(data.ImageFile.FileName);
                        fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                        data.brand_image = "~/Images/BrandImages/" + fileName;
                        fileName = Path.Combine(Server.MapPath("~/Images/BrandImages"), fileName);
                        data.ImageFile.SaveAs(fileName);
                    }

                    var brand = Mapper.Map<brand>(data);
                    context.brands.Add(brand);
                    context.SaveChanges();
                    TempData["SuccessMessage"] = "brand created successfully!";
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(ex.Message, "An error occurred while creating the brand. Please try again.");
                }
            }
            return View(data);
        }
        // GET: brands/Edit
        public ActionResult Edit(int? id)
        {
            if (id > 0)
            {
                var brand = context.brands.SingleOrDefault(c => c.brand_id == id);

                if (brand!=null)
                {
                    BrandViewModel catvm = Mapper.Map<BrandViewModel>(brand);
                    return View(catvm);
                }
                TempData["NotFound"] = "brand not found!";
                return View("Error");
            }
            return View("Error");
        }

        // POST: brands/Edit
        [HttpPost]
        public ActionResult Edit(BrandViewModel data)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var brandEntity = context.brands.Find(data.brand_id);
                    if (brandEntity == null)
                    {
                        return HttpNotFound("Brand not found!");
                    }
                    if (data.ImageFile != null && data.ImageFile.ContentLength > 0)
                    {
                        string fileName = Path.GetFileNameWithoutExtension(data.ImageFile.FileName);
                        string extension = Path.GetExtension(data.ImageFile.FileName);
                        fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                        data.brand_image = "~/Images/BrandImages/" + fileName;
                        fileName = Path.Combine(Server.MapPath("~/Images/BrandImages/"), fileName);
                        data.ImageFile.SaveAs(fileName);

                       
                        brandEntity.brand_image = data.brand_image;
                    }
                    brandEntity.brand_name = data.brand_name;

                    context.Entry(brandEntity).State = EntityState.Modified;
                    context.SaveChanges();

                    TempData["SuccessMessage"] = "Brand updated successfully!";
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(ex.Message, "An error occurred while updating the brand. Please try again.");
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
                    var brand = context.brands.Find(id);
                    if (brand == null)
                    {
                        return Json(new { success = false, message = "brand not found." });
                    }

                    context.brands.Remove(brand);
                    context.SaveChanges();

                    return Json(new { success = true, message = "brand deleted successfully!" });
                }
                catch (Exception ex)
                {                   
                    return Json(new { success = false, message = "An error occurred while deleting the brand. Please try again." });
                }
            }

            return Json(new { success = false, message = "Invalid brand ID." });
        }
    }
}