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
                new Student(){Id= 6,Name = "Test6",Age = 5},
                new Student(){Id= 7,Name = "Test7",Age = 4},
                new Student(){Id= 8,Name = "Test8",Age = 8},
                new Student(){Id= 9,Name = "Test9",Age = 3},
                new Student(){Id= 10,Name = "Test10",Age = 12},
                new Student(){Id= 11,Name = "Test11",Age = 16},
                new Student(){Id= 12,Name = "Test12",Age = 17},
                new Student(){Id= 13,Name = "Test13",Age = 13},
                new Student(){Id= 14,Name = "Test14",Age = 15},
                new Student(){Id= 15,Name = "Test15",Age = 14},
                new Student(){Id= 16,Name = "Test16",Age = 18}
            };
            return View(student);
        }

        public PartialViewResult getStudents()
        {
            List<Student> student = new List<Student>()
            {
                new Student(){Id= 1,Name = "Test1",Age = 3},
                new Student(){Id= 2,Name = "Test2",Age = 2},
                new Student(){Id= 3,Name = "Test3",Age = 6},
                new Student(){Id= 4,Name = "Test4",Age = 7},
                new Student(){Id= 5,Name = "Test5",Age = 3},
                new Student(){Id= 6,Name = "Test6",Age = 5},
                new Student(){Id= 7,Name = "Test7",Age = 4},
                new Student(){Id= 8,Name = "Test8",Age = 8},
                new Student(){Id= 9,Name = "Test9",Age = 3},
                new Student(){Id= 10,Name = "Test10",Age = 12},
                new Student(){Id= 11,Name = "Test11",Age = 16},
                new Student(){Id= 12,Name = "Test12",Age = 17},
                new Student(){Id= 13,Name = "Test13",Age = 13},
                new Student(){Id= 14,Name = "Test14",Age = 15},
                new Student(){Id= 15,Name = "Test15",Age = 14},
                new Student(){Id= 16,Name = "Test16",Age = 18}
            };
            return PartialView("_jQuery",student);
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