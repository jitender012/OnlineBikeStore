﻿using OnlineBikeStore.Extensions;
using OnlineBikeStore.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace OnlineBikeStore.Controllers
{
    public class FeedbackController : Controller
    {
        BikeStoreEntities context;
        public FeedbackController()
        {
            context = new BikeStoreEntities();
        }

        // GET: Get all Feedbacks if user is admin else particular customer feedbacks
        public ActionResult Index()
        {
            //get all feedbacks
            var feedbacks = (from f in context.feedbacks
                             join p in context.products on f.product_id equals p.product_id
                             select new FeedbackViewModel
                             {
                                 customer_id = f.customer_id,
                                 date = f.date,
                                 feedback_id = f.feedback_id,
                                 feedback_text = f.feedback_text,
                                 image_url = f.image_url,
                                 product_id = f.product_id,
                                 ratingValue = f.ratingValue,
                                 product_img = p.url,
                                 product_name = p.product_name
                             }).ToList();

            //get all feedbacks of a customer 
            if (User.IsInRole("customer"))
            {
                var userId = context.GetUserId(User.Identity.Name);

                feedbacks = feedbacks.Where(f => f.customer_id == userId).ToList();
                return PartialView("_UserReviews", feedbacks);
            }
            if (User.IsInRole("admin"))
            {
                return View(feedbacks);
            }
            else
            {
                return View("Error");
            }
        }
        
        // GET: Get all Feedbacks of a particular product
        [HttpGet]        
        public ActionResult ProductReviews(int pId)
        {
            var feedbacks = (from f in context.feedbacks
                             join p in context.products on f.product_id equals p.product_id
                             where f.product_id == pId
                             select new FeedbackViewModel2
                             {
                                 product_img = p.url,
                                 product_name = p.product_name,
                                 feedbacks = context.feedbacks
                                 .Where(fb => fb.product_id == pId)
                                 .Select(fb => new FeedbackViewModel
                                 {
                                     customer_id = fb.customer_id,
                                     feedback_id = fb.feedback_id,
                                     customer_name = context.users
                                                        .Where(x => x.user_id == fb.customer_id)
                                                        .Select(z => z.first_name + " " + z.last_name)
                                                        .FirstOrDefault(),
                                     date = fb.date,
                                     feedback_text = fb.feedback_text,
                                     image_url = fb.image_url,
                                     ratingValue = fb.ratingValue

                                 }).ToList()
                             })
                             .FirstOrDefault();
            return View(feedbacks);
        }
        [Authorize]
        public ActionResult AddUpdateFeedback(int? pId)
        {
            if (pId < 1 || pId == null)
            {
                return View("Error");
            }
            int uId = context.GetUserId(User.Identity.Name);

            //check if review existed
            bool isReviewed = context.feedbacks
                .Any(f => f.product_id == pId && f.customer_id == uId);

            product product = context.products
                .Where(p => p.product_id == pId)
                .SingleOrDefault();

            var feedbackData = new FeedbackViewModel()
            {
                product_name = product.product_name,
                product_img = product.url,
                product_id = pId.Value
            };

            //set values for fields if review existed
            if (isReviewed)
            {
                feedbackData = context.feedbacks
                   .Where(f => f.product_id == pId && f.customer_id == uId).Select(x => new FeedbackViewModel
                   {
                       feedback_id = x.feedback_id,
                       customer_id = x.customer_id,
                       product_id = x.product_id,
                       image_url = x.image_url,
                       ratingValue = x.ratingValue,
                       feedback_text = x.feedback_text,
                       date = x.date,
                       product_name = product.product_name,
                       product_img = product.url,
                   })
                   .SingleOrDefault();

                return View(feedbackData);
            }
            return View(feedbackData);
        }

        [Authorize]
        [HttpPost]
        public ActionResult AddUpdateFeedback(FeedbackViewModel data)
        {
            var userId = context.GetUserId(User.Identity.Name);
            if (!ModelState.IsValid)
            {
                return View(data);
            }
            var isItemDelivered = context.orders
                        .Any(o => o.customer_id == userId
                        && o.order_status == 2
                        && o.order_items.Any(oi => oi.product_id == data.product_id));

            var isReviewed = context.feedbacks
                                .Where(x => x.product_id == data.product_id && x.customer_id == userId)
                                .Any();

            //Varify that item is delivered
            if (isItemDelivered)
            {
                //Add review if not reviewed
                if (!isReviewed)
                {
                    try
                    {
                        if (data.ImageFile != null && data.ImageFile.ContentLength > 0)
                        {
                            // Save the image to the server
                            string fileName = Path.GetFileNameWithoutExtension(data.ImageFile.FileName);
                            string extension = Path.GetExtension(data.ImageFile.FileName);
                            fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                            data.image_url = "/Images/FeedbackImages/" + fileName;
                            fileName = Path.Combine(Server.MapPath("~/Images/FeedbackImages"), fileName);
                            data.ImageFile.SaveAs(fileName);
                        }

                        feedback newFeedback = new feedback()
                        {
                            customer_id = userId,
                            date = DateTime.Now,
                            feedback_text = data.feedback_text,
                            image_url = data.image_url,
                            product_id = data.product_id,
                            ratingValue = data.ratingValue,
                        };

                        context.feedbacks.Add(newFeedback);
                        context.SaveChanges();

                        return RedirectToAction("FeedbackSuccess");
                    }
                    catch (Exception)
                    {
                        return View(data);
                    }
                }

                //Edit review if already reviewed
                else
                {
                    var existingFeedback = context.feedbacks
                                                .Where(x => x.product_id == data.product_id && x.customer_id == userId)
                                                .FirstOrDefault();

                    if (data.ImageFile != null && data.ImageFile.ContentLength > 0)
                    {
                        // Save the image to the server
                        string fileName = Path.GetFileNameWithoutExtension(data.ImageFile.FileName);
                        string extension = Path.GetExtension(data.ImageFile.FileName);
                        fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                        data.image_url = "/Images/FeedbackImages/" + fileName;
                        fileName = Path.Combine(Server.MapPath("~/Images/FeedbackImages"), fileName);
                        data.ImageFile.SaveAs(fileName);
                    }

                    existingFeedback.date = DateTime.Now;
                    existingFeedback.feedback_text = data.feedback_text;
                    existingFeedback.ratingValue = data.ratingValue;
                    existingFeedback.image_url = data.image_url;

                    context.SaveChanges();

                    return RedirectToAction("FeedbackSuccess");
                }
            }
            return RedirectToAction("UnauthorisedAccessError", "Error", new { msg = "You have not purchased this item." });
        }

        public ActionResult FeedbackSuccess()
        {
            var unratedProducts = GetUnratedProducts();
            return View(unratedProducts);
        }

        [HttpPost]
        public JsonResult RemoveFeedback(int? feedbackId)
        {
            if (feedbackId < 1 || feedbackId == null)
            {
                return Json(new { success = false, msg = "Invalid Feedback." }, JsonRequestBehavior.AllowGet);
            }
            var feedback = context.feedbacks.FirstOrDefault(f => f.feedback_id == feedbackId);
            if (feedback != null)
            {
                context.feedbacks.Remove(feedback);
                context.SaveChanges();

                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { success = false, msg = "Feedback not found." }, JsonRequestBehavior.AllowGet);
        }

        public List<ProductViewModel> GetUnratedProducts()
        {
            var userId = context.GetUserId(User.Identity.Name);

            //get products that are not reviewed by logged in customer using stored procedure
            var unratedProducts = context.spGetUnratedProducts(userId)
                .Select(x => new ProductViewModel
                {
                    product_id = x.product_id,
                    product_name = x.product_name,
                    url = x.url
                }).ToList();

            return unratedProducts;
        }
    }
}