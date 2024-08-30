using OnlineBikeStore.Extensions;
using OnlineBikeStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineBikeStore.Controllers
{
    public class TestController : Controller
    {
        BikeStoreEntities context;
        // GET: Test
        public TestController()
        {
            context = new BikeStoreEntities();
        }
        public ActionResult Test()
        {          
            return View();
        }
        public ActionResult jQuery()
        {
            return View();
        }
        public ActionResult Method(FeedbackViewModel data)
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddFeedback(FeedbackViewModel data)
        {
            var userId = context.GetUserId(User.Identity.Name);
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

            return RedirectToAction("Home","home");
        }
    }
}