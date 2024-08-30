using AutoMapper;
using Newtonsoft.Json;
using OnlineBikeStore.Extensions;
using OnlineBikeStore.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.IO;
using System.Linq;
using System.Web;
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

        // GET: Get all Feedbacks if userId is null else particular cutomer feedbacks
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
        [HttpGet]
        public JsonResult ProductReviews(int pId)
        {
            var feedbacks = (from f in context.feedbacks
                             join p in context.products on f.product_id equals p.product_id
                             where f.product_id == pId
                             select new FeedbackViewModel
                             {
                                 customer_id = f.customer_id,
                                 feedback_id = f.feedback_id,
                                 customer_name = context.users
                                                    .Where(x => x.user_id == f.customer_id)
                                                    .Select(z => z.first_name + " " + z.last_name)
                                                    .FirstOrDefault(),
                                 date = f.date,
                                 feedback_text = f.feedback_text,
                                 image_url = f.image_url,
                                 ratingValue = f.ratingValue,
                             }).ToList();
            return Json(feedbacks, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AddUpdateFeedback(FeedbackViewModel data)
        {
            var userId = context.GetUserId(User.Identity.Name);


            if (data.feedback_id < 1)
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

                    return Json(new { success = true }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                var existingFeedback = context.feedbacks
                                            .Where(x => x.feedback_id == data.feedback_id)
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
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }

        }


        [HttpPost]
        public JsonResult EditFeedback(FeedbackViewModel data)
        {
            var feedback = context.feedbacks.Where(x => x.feedback_id == data.feedback_id).SingleOrDefault();
            if (feedback != null)
            {
                if (data.ImageFile != null && data.ImageFile.ContentLength > 0)
                {
                    // Save  image to the server
                    string fileName = Path.GetFileNameWithoutExtension(data.ImageFile.FileName);
                    string extension = Path.GetExtension(data.ImageFile.FileName);
                    fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                    data.image_url = "~/Images/FeedbackImages/" + fileName;
                    fileName = Path.Combine(Server.MapPath("~/Images/FeedbackImages"), fileName);
                    data.ImageFile.SaveAs(fileName);
                }
                feedback.ratingValue = data.ratingValue;
                feedback.feedback_text = data.feedback_text;
                feedback.date = data.date;
                feedback.image_url = data.image_url;

                context.SaveChanges();

                return Json(new { feedback, success = true }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { success = false }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult RemoveFeedback(int feedbackId)
        {
            if (feedbackId > 0)
            {
                var feedback = context.feedbacks.FirstOrDefault(f => f.feedback_id == feedbackId);
                if (feedback != null)
                {
                    context.feedbacks.Remove(feedback);
                    context.SaveChanges();

                    return Json(new { success = true }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { success = true, msg = "Feedback not found." }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { success = true, msg = "Invalid Feedback." }, JsonRequestBehavior.AllowGet);
        }

    }
}