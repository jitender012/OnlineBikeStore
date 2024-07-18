using AutoMapper;
using OnlineBikeStore.Models;
using System;
using System.Collections.Generic;
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
        // GET: Feedback
        public ActionResult Index(int userId = 0, int productId = 0)
        {
            if (userId > 0)
            {
                var userReviews = context.feedbacks.Where(f => f.customer_id == userId).ToList();
                return View(Mapper.Map<List<FeedbackViewModel>>(userReviews));
            }
            if (productId > 0)
            {
                var productReviews = context.feedbacks.Where(f => f.product_id == productId).ToList();
                return View(Mapper.Map<List<FeedbackViewModel>>(productReviews));
            }
            return View();
        }



    }
}