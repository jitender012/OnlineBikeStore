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
    public class ProductController : Controller
    {
        readonly BikeStoreEntities context;
        public ProductController()
        {
            context = new BikeStoreEntities();
        }
        // GET: Products
        public ActionResult Index()
        {
            var products = context.products.ToList();
            List<ProductViewModel> productView = Mapper.Map<List<ProductViewModel>>(products);
            return View(productView);
        }

        // GET: Products
        public ActionResult Details(int id)
        {
            //Find product in database
            var product = context.products.SingleOrDefault(c => c.product_id == id);
            var category = context.categories.SingleOrDefault(x => x.category_id == product.category_id);
            var brand = context.brands.SingleOrDefault(x => x.brand_id == product.brand_id);
            // Check if the product found
            if (product == null)
            {
                return HttpNotFound("Product not found.");
            }

            //Map database product to view model
            ProductDetailsViewModel productDetailsViewModel = new ProductDetailsViewModel()
            {
                product_name = product.product_name,
                product_id = product.product_id,
                list_price = product.list_price,
                product_type = product.product_type,
                description = product.description,
                url = product.url,
                model_year = product.model_year,
                category_id = product.category_id,
                brand_id = product.brand_id,
                category_name = category.category_name,
                brand_name = brand.brand_name,                
            };


            return View(productDetailsViewModel);
        }
        private SelectList GetSelectList<TEntiyModel, TViewModel>(IEnumerable<TEntiyModel> domainModels, string dataValueField, string dataTextField)
        {
            var viewModelList = Mapper.Map<List<TViewModel>>(domainModels);
            return new SelectList(viewModelList, dataValueField, dataTextField);
        }
        // GET: Products/Create
        public ActionResult Create()
        {
            var categories = context.categories.ToList();
            var brands = context.brands.ToList();
            ViewBag.BrandList = GetSelectList<brand, BrandViewModel>(brands, "brand_id", "brand_name");
            ViewBag.CategoryList = GetSelectList<category, CategoryViewModel>(categories, "category_id", "category_name");
            return View();
        }

        // POST: Products/Create
        [HttpPost]
        public ActionResult Create(ProductViewModel data)
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
                        data.url = "~/Images/ProductImages/" + fileName;
                        fileName = Path.Combine(Server.MapPath("~/Images/ProductImages"), fileName);
                        data.ImageFile.SaveAs(fileName);
                    }

                    var product = Mapper.Map<product>(data);
                    context.products.Add(product);
                    context.SaveChanges();
                    TempData["SuccessMessage"] = "Product created successfully!";
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {

                    ModelState.AddModelError(ex.Message, "An error occurred while creating the product. Please try again.");
                }
            }
            var categories = context.categories.ToList();
            var brands = context.brands.ToList();
            ViewBag.BrandList = GetSelectList<brand, BrandViewModel>(brands, "brand_id", "brand_name");
            ViewBag.CategoryList = GetSelectList<category, CategoryViewModel>(categories, "category_id", "category_name");
            return View(data);
        }
        // GET: Products/Edit
        public ActionResult Edit(int? id)
        {
            var categories = context.categories.ToList();
            var brands = context.brands.ToList();
            ViewBag.BrandList = GetSelectList<brand, BrandViewModel>(brands, "brand_id", "brand_name");
            ViewBag.CategoryList = GetSelectList<category, CategoryViewModel>(categories, "category_id", "category_name");
            if (id > 0)
            {
                var product = context.products.SingleOrDefault(c => c.product_id == id);

                if (product != null)
                {
                    ProductViewModel prodvm = Mapper.Map<ProductViewModel>(product);
                    return View(prodvm);
                }
                TempData["NotFound"] = "Product not found!";
                return View("Error");
            }
            return View("Error");
        }

        // POST: Products/Edit
        [HttpPost]
        public ActionResult Edit(ProductViewModel data)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var productEntity = context.products.Find(data.product_id);
                    if (productEntity == null)
                    {
                        return HttpNotFound();
                    }
                    if (data.ImageFile != null && data.ImageFile.ContentLength > 0)
                    {
                        string fileName = Path.GetFileNameWithoutExtension(data.ImageFile.FileName);
                        string extension = Path.GetExtension(data.ImageFile.FileName);
                        fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                        data.url = "~/Images/ProductImages/" + fileName;
                        fileName = Path.Combine(Server.MapPath("~/Images/ProductImages/"), fileName);
                        data.ImageFile.SaveAs(fileName);


                        productEntity.url = data.url;
                    }
                    productEntity.product_name = data.product_name;
                    productEntity.list_price = data.list_price;
                    productEntity.brand_id = data.brand_id;
                    productEntity.category_id = data.category_id;
                    productEntity.description = data.description;
                    productEntity.model_year = data.model_year;
                    productEntity.product_type = data.product_type;                   
                    
                    context.Entry(productEntity).State = EntityState.Modified;
                    context.SaveChanges();

                    TempData["SuccessMessage"] = "Product updated successfully!";
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {

                    ModelState.AddModelError(ex.Message, "An error occurred while updating the product. Please try again.");
                }
            }

            var categories = context.categories.ToList();
            var brands = context.brands.ToList();
            ViewBag.BrandList = GetSelectList<brand, BrandViewModel>(brands, "brand_id", "brand_name");
            ViewBag.CategoryList = GetSelectList<category, CategoryViewModel>(categories, "category_id", "category_name");
            return View(data);
        }



        [HttpPost]
        public JsonResult Delete(int id)
        {
            if (id > 0)
            {
                try
                {
                    var product = context.products.Find(id);
                    if (product == null)
                    {
                        return Json(new { success = false, message = "Product not found." });
                    }

                    context.products.Remove(product);
                    context.SaveChanges();

                    return Json(new { success = true, message = "Product deleted successfully!" });
                }
                catch (Exception)
                {
                    // Log the exception here for further analysis if needed
                    return Json(new { success = false, message = "An error occurred while deleting the product. Please try again." });
                }
            }

            return Json(new { success = false, message = "Invalid product ID." });
        }

    }
}