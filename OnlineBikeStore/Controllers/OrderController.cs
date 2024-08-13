using OnlineBikeStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineBikeStore.Controllers
{
    public class OrderController : Controller
    {
        BikeStoreEntities context;
        public OrderController()
        {
            context = new BikeStoreEntities();
        }
        // GET: Order
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult OrderSummary(int pId = 0)
        {
            var userId = context.users.Where(x => x.email == User.Identity.Name).Select(u => u.user_id).SingleOrDefault();
            if (pId > 0)
            {
                order newOrder = new order()
                {
                    customer_id = userId,
                    order_status = (byte)OrderStatus.Placed,
                    order_date = DateTime.Now,
                    required_date = DateTime.Now.AddDays(7),
                };
                // Add the new order to the context and save changes
                context.orders.Add(newOrder);
                context.SaveChanges();

                order_items _Item = new order_items()
                {
                    order_id = newOrder.order_id,
                    product_id = pId,
                    quantity = 1,
                    list_price = 0,
                };
            }
            return View();
        }

        public ActionResult PaymentSuccess() 
        {
            return View(); 
        }
    }
}