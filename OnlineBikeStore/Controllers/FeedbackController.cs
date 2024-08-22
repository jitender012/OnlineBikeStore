using AutoMapper;
using OnlineBikeStore.Extensions;
using OnlineBikeStore.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineBikeStore.Controllers
{
    [Authorize]
    public class FeedbackController : Controller
    {
        BikeStoreEntities context;
        public FeedbackController()
        {
            context = new BikeStoreEntities();
        }
        // GET: Feedback
        public ActionResult FeedbackIndex()
        {
            var userId = context.GetUserId(User.Identity.Name);
            var feedbacks = context.feedbacks.Where(f => f.customer_id == userId).ToList();

            var feedbackViewModel = (from f in context.feedbacks
                                     join p in context.products on f.product_id equals p.product_id
                                     where f.customer_id == userId
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
            return View(feedbackViewModel);
        }
        [HttpPost]
        public JsonResult AddFeedback(FeedbackViewModel data)
        {
            var userId = context.GetUserId(User.Identity.Name);
            if (ModelState.IsValid)
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

                return Json(new { newFeedback, success = true }, JsonRequestBehavior.AllowGet);
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