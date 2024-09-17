using AutoMapper;
using Microsoft.Ajax.Utilities;
using OnlineBikeStore.Extensions;
using OnlineBikeStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;

namespace OnlineBikeStore.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        BikeStoreEntities context;
        public OrderController()
        {
            context = new BikeStoreEntities();
        }

        // GET: List of all Orders
        public ActionResult GetOrders()
        {
            //Get logged in user id
            var userId = context.GetUserId(User.Identity.Name);

            //Get all orders from database and map to the OrderViewModel
            List<OrderViewModel> ordersListVM = context.orders
                .Select(z => new OrderViewModel
                {
                    customer_id = z.customer_id.Value,
                    order_date = z.order_date,
                    order_id = z.order_id,
                    order_status = z.order_status,
                    required_date = z.required_date,
                    shipped_date = z.shipped_date,
                    total_price = z.order_items.Sum(a => a.list_price),
                    item_names = z.order_items.Select(x => x.product.product_name).ToList(),
                    first_product_image = z.order_items.Select(x => x.product.url).FirstOrDefault(),
                    total_items = z.order_items.Count()
                })
                .ToList();

            //Check if logged in user is customer
            if (User.IsInRole("customer"))
            {
                //Filter the orders based on customer id
                ordersListVM = ordersListVM
                    .Where(x => x.customer_id == userId)
                    .ToList();
                return View("GetOrdersCustomer", ordersListVM);
            }
            return View("GetOrdersAdmin", ordersListVM);
        }

        [Authorize]
        [HttpGet]
        public PartialViewResult GetOrdersPartial(string query, string orderStatus)
        {
            var userId = context.GetUserId(User.Identity.Name);

            //Get all orders
            var ordersListVM = context.orders
               .Select(z => new OrderViewModel
               {
                   customer_id = z.customer_id.Value,
                   order_date = z.order_date,
                   order_id = z.order_id,
                   order_status = z.order_status,
                   required_date = z.required_date,
                   shipped_date = z.shipped_date,
                   total_price = z.order_items.Sum(a => a.list_price),
                   item_names = z.order_items.Select(x => x.product.product_name).ToList(),
                   first_product_image = z.order_items.Select(x => x.product.url).FirstOrDefault(),
                   total_items = z.order_items.Count()

               }).ToList();

            //Search orders using order id
            if (query != null)
            {
                ordersListVM = ordersListVM
                    .Where(o =>
                    o.item_names != null && o.item_names.Any(name => name.IndexOf(query, StringComparison.OrdinalIgnoreCase) >= 0) ||
                    o.order_date.ToString("D").Contains(query) ||
                    o.total_price.ToString().Contains(query) ||
                    o.order_id.ToString().Contains(query)
                    )
                    .ToList();
            }
            var o_status = -1;

            if (orderStatus != "")
            {
                switch (orderStatus)
                {
                    case "Placed":
                        o_status = 0;
                        break;
                    case "Shipped":
                        o_status = 1;
                        break;
                    case "Delivered":
                        o_status = 2;
                        break;
                    case "Cancelled":
                        o_status = 3;
                        break;
                    case "Returned":
                        o_status = 4;
                        break;

                    case "allOrders":
                        o_status = -5;
                        break;
                    default:
                        o_status = 6;
                        break;
                }
            }

            //Filter orders by status
            if (o_status >= 0)
            {
                ordersListVM = ordersListVM
                    .Where(o => o.order_status == o_status)
                    .ToList();
            }

            if (User.IsInRole("customer"))
            {
                ordersListVM = ordersListVM
                    .Where(x => x.customer_id == userId)
                    .ToList();
                return PartialView("_GetOrdersCustomer", ordersListVM);
            }

            return PartialView("_GetOrdersAdmin", ordersListVM);

        }

        [Authorize]
        public ActionResult OrderDetails(int orderId)
        {
            //Get logged in user id
            var userId = context.GetUserId(User.Identity.Name);

            if (orderId > 0)
            {
                //get order details
                var orderDetailsViewModel = GetOrderDetails(orderId);

                //return to OrderDetailsCustomer view if user is customer
                if (User.IsInRole("customer"))
                {
                    //ensure that customer can see thier own order details                   
                    if (orderDetailsViewModel.userDetails.user_id != userId)
                    {
                        return View("error");
                    }
                    return View("OrderDetailsCustomer", orderDetailsViewModel);
                }

                //return to OrderDetailsAdmin view if user is admin
                return View("OrderDetailsAdmin", orderDetailsViewModel);
            }
            return View("Error");
        }

        public OrderDetailsViewModel GetOrderDetails(int orderId)
        {
            var orderDetailsVM = context.orders
                .Where(o => o.order_id == orderId)
                .Select(ovm => new OrderDetailsViewModel
                {
                    order_id = ovm.order_id,
                    order_status = ovm.order_status,
                    order_date = ovm.order_date,
                    required_date = ovm.required_date,
                    userDetails = new UserViewModel
                    {
                        user_id = ovm.user.user_id,
                        first_name = ovm.user.first_name,
                        last_name = ovm.user.last_name,
                        city = ovm.user.city,
                        email = ovm.user.email,
                        phone = ovm.user.phone,
                        street = ovm.user.street,
                        state = ovm.user.state,
                        zip_code = ovm.user.zip_code
                    },
                    orderItems = ovm.order_items.Select(oi => new OrderItem
                    {
                        item_id = oi.item_id,
                        quantity = oi.quantity,
                        productDetails = new ProductDetailsViewModel
                        {
                            product_id = oi.product.product_id,
                            product_name = oi.product.product_name,
                            list_price = oi.product.list_price,
                            url = oi.product.url,
                            description = oi.product.description
                        }
                    })
                    .ToList()
                })
                .SingleOrDefault();
            return orderDetailsVM;
        }
        public JsonResult UpdateOrderStatus(int oID, string oStatus)
        {
            byte orderStatus = byte.Parse(oStatus);
            if (oID > 0)
            {
                var order = context.orders
                                .SingleOrDefault(o => o.order_id == oID);
                if (order == null)
                {
                    return Json(new { success = false, message = "Order Not Found!" }, JsonRequestBehavior.AllowGet);
                }
                order.order_status = orderStatus;

                if (orderStatus == 1)
                {
                    order.shipped_date = DateTime.Now;
                }
                //required date will be used as cancelled and delivered date
                if (orderStatus == 3 || orderStatus == 2)
                {
                    order.required_date = DateTime.Now;
                }
                context.SaveChanges();

                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { success = false, message = "Invalid Order Id!" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult OrderSummary(int pId = 0)
        {
            if (User.Identity.IsAuthenticated)
            {
                //get the logged in user id
                var userId = context.users
                                .Where(x => x.email == User.Identity.Name)
                                .Select(u => u.user_id)
                                .SingleOrDefault();

                // Map the user to the UserViewModel
                var userVm = Mapper.Map<UserViewModel>(context.users
                                .Where(u => u.user_id == userId)
                                .SingleOrDefault());

                //for single product order
                if (pId > 0)
                {
                    var cartProducts = context.products
                                .Where(p => p.product_id == pId)
                                .ToList();

                    // Map the products to the ProductViewModel
                    var cartProductsVM = Mapper.Map<List<ProductViewModel>>(cartProducts);

                    OrderSummaryViewModel orderSummary = new OrderSummaryViewModel()
                    {
                        user = userVm,
                        products = cartProductsVM
                    };
                    return View(orderSummary);
                }
                //when order through cart
                else
                {
                    // Get the user's cart items
                    var cartItems = context.userCarts
                                .Where(c => c.user_id == userId)
                                .ToList();

                    //get product id's from user cart
                    var productIds = cartItems.Select(q => q.product_id).ToList();

                    // Fetch the products from the product IDs
                    var cartProducts = context.products
                                .Where(p => productIds.Contains(p.product_id))
                                .ToList();

                    // Map the products to the ProductViewModel
                    var cartProductsVM = Mapper.Map<List<ProductViewModel>>(cartProducts);

                    OrderSummaryViewModel orderSummary = new OrderSummaryViewModel()
                    {
                        user = userVm,
                        products = cartProductsVM
                    };

                    return View(orderSummary);
                }
            }
            else
            {
                TempData["RedirectToLoginMsg"] = "Login to purchase.";
                return RedirectToAction("Login", "Account");
            }
        }

        [HttpPost]
        public ActionResult PlaceOrder(int pId = 0)
        {
            if (User.Identity.IsAuthenticated)
            {
                //get the logged in user id
                var userId = context.users
                                .Where(x => x.email == User.Identity.Name)
                                .Select(u => u.user_id)
                                .SingleOrDefault();

                // Map the user to the UserViewModel
                var userVm = Mapper.Map<UserViewModel>(context.users
                                .Where(u => u.user_id == userId)
                                .SingleOrDefault());

                //for single product order
                if (pId > 0)
                {
                    var product = context.products.FirstOrDefault(p => p.product_id == pId);

                    order newOrder = new order()
                    {
                        customer_id = userId,
                        order_status = (byte)OrderStatus.Placed,
                        order_date = DateTime.Now,
                        required_date = DateTime.Now.AddDays(7),
                    };
                    context.orders.Add(newOrder);
                    context.SaveChanges();

                    var orderId = newOrder.order_id;
                    if (orderId > 0)
                    {
                        order_items item = new order_items()
                        {
                            order_id = orderId,
                            product_id = pId,
                            quantity = 1,
                            list_price = product.list_price,
                            discount = 0
                        };
                        context.order_items.Add(item);
                        context.SaveChanges();

                        return RedirectToAction("OrderSuccess");
                    }

                }
                //when order through cart
                else
                {
                    order newOrder = new order()
                    {
                        customer_id = userId,
                        order_status = (byte)OrderStatus.Placed,
                        order_date = DateTime.Now,
                        required_date = DateTime.Now.AddDays(7),
                    };
                    context.orders.Add(newOrder);
                    context.SaveChanges();

                    var orderId = newOrder.order_id;

                    if (orderId > 0)
                    {
                        // Get the user's cart items
                        var cartItems = context.userCarts
                            .Where(c => c.user_id == userId)
                            .ToList();

                        //get product id's from user cart
                        var productIds = cartItems.Select(q => q.product_id).ToList();

                        // Fetch the products from the product IDs
                        var cartProducts = context.products
                                                  .Where(p => productIds.Contains(p.product_id))
                                                  .ToList();
                        foreach (var product in cartProducts)
                        {
                            order_items item = new order_items()
                            {
                                order_id = orderId,
                                product_id = product.product_id,
                                quantity = 1,
                                list_price = product.list_price,
                                discount = 0
                            };
                            context.order_items.Add(item);
                            userCart cart = context.userCarts
                                    .FirstOrDefault(c => c.user_id == userId && c.product_id == product.product_id);
                            context.userCarts.Remove(cart);
                            context.SaveChanges();
                        }
                        return RedirectToAction("OrderSuccess");
                    }
                }
            }
            else
            {
                TempData["RedirectToLoginMsg"] = "Login to purchase.";
                return RedirectToAction("Login", "Account");
            }

            return View();
        }

        public ActionResult OrderSuccess()
        {
            return View();
        }
    }
}