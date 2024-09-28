using AutoMapper;
using OnlineBikeStore.Extensions;
using OnlineBikeStore.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
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
        [Authorize(Roles ="admin")]
        public ActionResult Index()
        {
            var products = context.products.ToList();
            List<ProductViewModel> productView = Mapper.Map<List<ProductViewModel>>(products);
            return View(productView);
        }

        // GET: Products
        [Authorize(Roles = "admin")]
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
        [Authorize(Roles = "admin")]
        // GET: Products/Create
        public ActionResult Create()
        {
            var categories = context.categories.ToList();
            var brands = context.brands.ToList();
            ViewBag.BrandList = GetSelectList<brand, BrandViewModel>(brands, "brand_id", "brand_name");
            ViewBag.CategoryList = GetSelectList<category, CategoryViewModel>(categories, "category_id", "category_name");
            return View();
        }
        [Authorize(Roles = "admin")]
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
        [Authorize(Roles = "admin")]
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
        [Authorize(Roles = "admin")]
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


        [Authorize(Roles = "admin")]
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


        public ActionResult Product(int? pId)
        {
            if (pId <= 0 && pId == null)
            {
                return View("Error");
            }

            // Check if the product found
            var product = context.products
                .SingleOrDefault(c => c.product_id == pId);
            if (product == null)
            {
                return HttpNotFound("Product not found.");
            }

            var categories = context.categories.ToList();
            ViewBag.Categories = Mapper.Map<List<CategoryViewModel>>(categories);


            var category = context.categories
                .SingleOrDefault(x => x.category_id == product.category_id);
            var brand = context.brands
                .SingleOrDefault(x => x.brand_id == product.brand_id);

            bool isInCart = false;
            bool isInWishlist = false;
            bool isPurchased = false;
            bool isReviewed = false;
            ViewBag.UserId = 0;


            if (User.Identity.IsAuthenticated)
            {
                int uId = context.GetUserId(User.Identity.Name);

                var userData = context.users
                       .Include(u => u.userCarts)
                       .Include(u => u.wishlists)
                       .Include(u => u.orders.Select(o => o.order_items))
                       .Include(u => u.feedbacks)
                       .SingleOrDefault(u => u.user_id == uId);

                if (userData != null)
                {
                    isInCart = userData.userCarts
                        .Any(c => c.product_id == pId);
                    isInWishlist = userData.wishlists
                        .Any(w => w.product_id == pId);
                    isPurchased = userData.orders
                        .Any(o => o.order_status == 2 && o.order_items.Any(oi => oi.product_id == pId));
                    isReviewed = userData.feedbacks
                        .Any(f => f.product_id == pId);

                    ViewBag.UserId = uId;
                }
            }

            var feedbackViewModel = context.feedbacks
                                    .Where(x => x.product_id == pId)
                                    .Select(f => new FeedbackViewModel
                                    {
                                        feedback_id = f.feedback_id,
                                        ratingValue = f.ratingValue,
                                        customer_id = f.customer_id,
                                        date = f.date,
                                        feedback_text = f.feedback_text,
                                        image_url = f.image_url,
                                        customer_name = context.users
                                                .Where(x => x.user_id == f.customer_id)
                                                .Select(z => z.first_name + " " + z.last_name)
                                                .FirstOrDefault()
                                    })
                                    .ToList();


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
                isInCart = isInCart,
                isInWishList = isInWishlist,
                isPurchased = isPurchased,
                isReviewed = isReviewed,
                Feedback = feedbackViewModel,
                currentStock = context.stocks
                    .Where(p => p.product_id == pId)
                    .Select(s => (int)s.quantity)
                    .DefaultIfEmpty(-1)
                    .FirstOrDefault()
            };

            return View(productDetailsViewModel);
        }
        public ActionResult Search(string searchWord)
        {
            var categories = context.categories.ToList();
            ViewBag.Categories = Mapper.Map<List<CategoryViewModel>>(categories);
            ViewBag.Brands = context.brands.Select(b => b.brand_name).ToList();


            if (searchWord != null)
            {
                ViewBag.searchString = searchWord;
                searchWord.ToLower();

                var products = context.products
                    .Where(p => p.product_name.Contains(searchWord) ||
                                p.description.Contains(searchWord) ||
                                p.brand.brand_name.Contains(searchWord) ||
                                p.category.category_name.Contains(searchWord) ||
                                p.product_type.Contains(searchWord))
                    .ToList();
                if (products != null)
                {
                    var productsVM = Mapper.Map<List<ProductViewModel>>(products);
                    return View(productsVM);
                }
            }
            var result = Mapper.Map<List<ProductViewModel>>(context.products.ToList());

            return View(result);
        }

        public ActionResult Filter(int catId)
        {
            var categories = context.categories.ToList();
            ViewBag.Categories = Mapper.Map<List<CategoryViewModel>>(categories);
            ViewBag.Brands = context.brands.Select(b => b.brand_name).ToList();
            var products = context.products.Where(p => p.category_id == catId).ToList();

            ViewBag.catId = catId;
            if (products != null)
            {
                var productsVM = Mapper.Map<List<ProductViewModel>>(products);
                return View("Search", productsVM);
            }
            return View("Error");
        }

        [HttpPost]
        public PartialViewResult SearchPartial(FilterDataModel filters, string searchString, string catId)
        {
            List<int> productIds;
            List<product> filteredItems;

            // Select products based on search string
            if (!string.IsNullOrEmpty(searchString))
            {
                searchString = searchString.ToLower();

                productIds = context.products
                    .Where(p => p.product_name.ToLower().Contains(searchString) ||
                                p.description.ToLower().Contains(searchString) ||
                                p.brand.brand_name.ToLower().Contains(searchString) ||
                                p.category.category_name.ToLower().Contains(searchString) ||
                                p.product_type.ToLower().Contains(searchString))
                    .Select(o => o.product_id)
                    .ToList();
            }
            else
            {
                // Select products by category if search string is empty
                productIds = context.products
                    .Where(p => p.category_id.ToString() == catId)
                    .Select(x => x.product_id)
                    .ToList();
            }

            // Fetch products based on productIds
            filteredItems = context.products
                .Where(p => productIds.Contains(p.product_id))
                .ToList();

            // Apply filters for brands and types in memory (after fetching the data)
            if (filters.brands != null && filters.brands.Any())
            {
                filteredItems = filteredItems
                    .Where(p => filters.brands.Contains(p.brand.brand_name))
                    .ToList();
            }

            if (filters.type != null && filters.type.Any())
            {
                filteredItems = filteredItems
                    .Where(p => filters.type.Contains(p.product_type))
                    .ToList();
            }

            // Map filtered items to view model
            var productsVM = Mapper.Map<List<ProductViewModel>>(filteredItems);
            return PartialView("_SearchPartial", productsVM);
        }


        //Search Products By Category
        public ActionResult FilterByCategory(int? categoryId)
        {
            var categories = context.categories.ToList();
            ViewBag.Categories = Mapper.Map<List<CategoryViewModel>>(categories);

            if (categoryId.HasValue && categoryId > 0)
            {
                var products = context.products.Where(p => p.category_id == categoryId).ToList();
                var productsVM = Mapper.Map<List<ProductViewModel>>(products);
                return View("Search", productsVM);
            }
            else return View("Search");
        }
    }
}