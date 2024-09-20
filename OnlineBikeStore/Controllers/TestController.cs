using OnlineBikeStore.Extensions;
using OnlineBikeStore.Models;
using Rotativa;
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
        public ActionResult pageDesign()
        {

            return View("~/Views/Feedback/FeedbackSuccess.cshtml");
        }
        public ActionResult Test()
        {
            return View();
        }

        public ActionResult jQuery()
        {
            List<Student> student = new List<Student>()
            {
                new Student(){Id= 1,Name = "Test1",Age = 3},
                new Student(){Id= 2,Name = "Test2",Age = 2},
                new Student(){Id= 3,Name = "Test3",Age = 6},
                new Student(){Id= 4,Name = "Test4",Age = 7},
                new Student(){Id= 5,Name = "Test5",Age = 3},
               
            };
            return View( student);
        }
        public PartialViewResult jQueryPartial()
        {
            List<Student> student = new List<Student>()
            {
                new Student(){Id= 1,Name = "Test1",Age = 3},
                new Student(){Id= 2,Name = "Test2",Age = 2},
                new Student(){Id= 3,Name = "Test3",Age = 6},
                new Student(){Id= 4,Name = "Test4",Age = 7},
                new Student(){Id= 5,Name = "Test5",Age = 3},           
            };
            return PartialView("_jQuery", student);
        }


  
        public ActionResult DownloadStudentDetails()
        {
            List<Student> student = new List<Student>()
            {
                new Student(){Id= 1,Name = "Test1",Age = 3},
                new Student(){Id= 2,Name = "Test2",Age = 2},
                new Student(){Id= 3,Name = "Test3",Age = 6},
                new Student(){Id= 4,Name = "Test4",Age = 7},
                new Student(){Id= 5,Name = "Test5",Age = 3},
                
            };
            var uniqueId = Guid.NewGuid();
            return new PartialViewAsPdf("_jQuery", student)
            {
                FileName = $"Order_{uniqueId}.pdf"
            };
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

            return RedirectToAction("Home", "home");
        }
    }
}