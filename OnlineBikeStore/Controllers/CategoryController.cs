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
    public class CategoryController : Controller
    {

        BikeStoreEntities context;
        public CategoryController()
        {
            context = new BikeStoreEntities();
        }
        // GET: Categories
        public ActionResult Index()
        {
            var categories = context.categories.ToList();
            List<CategoryViewModel> categoryView = Mapper.Map<List<CategoryViewModel>>(categories);
            return View(categoryView);
        }

        // GET: Categories
        public ActionResult Details(int categoryId)
        {
            //Find category in database
            var category = context.categories.SingleOrDefault(c => c.category_id == categoryId);

            // Check if the category found
            if (category == null)
            {
                return HttpNotFound("Category not found.");
            }

            //Map database category to view model
            var categoryViewModel = Mapper.Map<CategoryViewModel>(category);

            return View(categoryViewModel);
        }

        // GET: Categories/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Categories/Create
        [HttpPost]
        public ActionResult Create(CategoryViewModel data)
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
                        data.category_image = "~/Images/CategoryImages/" + fileName;
                        fileName = Path.Combine(Server.MapPath("~/Images/CategoryImages"), fileName);
                        data.ImageFile.SaveAs(fileName);
                    }

                    var category = Mapper.Map<category>(data);
                    context.categories.Add(category);
                    context.SaveChanges();
                    TempData["SuccessMessage"] = "Category created successfully!";
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {

                    ModelState.AddModelError(ex.Message, "An error occurred while creating the category. Please try again.");
                }
            }
            return View(data);
        }
        // GET: Categories/Edit
        public ActionResult Edit(int? id)
        {
            if (id > 0)
            {
                var category = context.categories.SingleOrDefault(c => c.category_id == id);

                if (category!=null)
                {
                    CategoryViewModel catvm = Mapper.Map<CategoryViewModel>(category);
                    return View(catvm);
                }
                TempData["NotFound"] = "Category not found!";
                return View("Error");
            }
            return View("Error");
        }

        // POST: Categories/Edit
        [HttpPost]
        public ActionResult Edit(CategoryViewModel data)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var categoryEntity = context.categories.Find(data.category_id);
                    if (categoryEntity == null)
                    {
                        return HttpNotFound();
                    }
                    if (data.ImageFile != null && data.ImageFile.ContentLength > 0)
                    {
                        string fileName = Path.GetFileNameWithoutExtension(data.ImageFile.FileName);
                        string extension = Path.GetExtension(data.ImageFile.FileName);
                        fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                        data.category_image = "~/Images/CategoryImages/" + fileName;
                        fileName = Path.Combine(Server.MapPath("~/Images/CategoryImages/"), fileName);
                        data.ImageFile.SaveAs(fileName);

                       
                        categoryEntity.category_image = data.category_image;
                    }
                    categoryEntity.category_name = data.category_name;

                    context.Entry(categoryEntity).State = EntityState.Modified;
                    context.SaveChanges();

                    TempData["SuccessMessage"] = "Category updated successfully!";
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {

                    ModelState.AddModelError(ex.Message, "An error occurred while updating the category. Please try again.");
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
                    var category = context.categories.Find(id);
                    if (category == null)
                    {
                        return Json(new { success = false, message = "Category not found." });
                    }

                    context.categories.Remove(category);
                    context.SaveChanges();

                    return Json(new { success = true, message = "Category deleted successfully!" });
                }
                catch (Exception)
                {
                    // Log the exception here for further analysis if needed
                    return Json(new { success = false, message = "An error occurred while deleting the category. Please try again." });
                }
            }

            return Json(new { success = false, message = "Invalid category ID." });
        }

    }
}