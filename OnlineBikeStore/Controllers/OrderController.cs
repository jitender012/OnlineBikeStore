using AutoMapper;
using OnlineBikeStore.Extensions;
using OnlineBikeStore.Models;
using Rotativa;
using System;
using System.Collections.Generic;
using System.Linq;
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
        [Authorize]
        public ActionResult GetOrders()
        {
            //Get logged in user id
            int userId = context.GetUserId(User.Identity.Name);

            //Get all orders 
            List<OrderViewModel> ordersListVM = GetOrdersViewModel();

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

        public PartialViewResult GetOrdersPartial(string orderStatus)
        {
            int userId = context.GetUserId(User.Identity.Name);

            //Get all orders
            var ordersListVM = GetOrdersViewModel();

            //Search orders using order id/price/name/date
          
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

            //Select only customer order if logged in user is customer
            if (User.IsInRole("customer"))
            {
                ordersListVM = ordersListVM
                    .Where(x => x.customer_id == userId)
                    .ToList();
                return PartialView("_GetOrdersCustomer", ordersListVM);
            }

            return PartialView("_GetOrdersAdmin", ordersListVM);
        }

        // Helper method to RETRIEVE ORDERS and map it to view model
        private List<OrderViewModel> GetOrdersViewModel()
        {
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
                }).ToList();
            return ordersListVM;
        }

        public ActionResult OrderDetails(int? orderId)
        {

            //Get logged in user id
            var userId = context.GetUserId(User.Identity.Name);

            if (orderId < 1 || orderId == null)
            {
                return View("Error");
            }
            //get order details
            var orderDetailsViewModel = GetOrderDetails(orderId.Value);

            //return to OrderDetailsCustomer view if user is customer
            if (User.IsInRole("customer"))
            {
                //ensure that customer can see thier own order details                   
                if (orderDetailsViewModel.userDetails.user_id != userId)
                {
                    return RedirectToAction("UnauthorisedAccessError","Error", new {msg=""});
                }
                return View("OrderDetailsCustomer", orderDetailsViewModel);
            }

            //return to OrderDetailsAdmin view if user is admin
            return View("OrderDetailsAdmin", orderDetailsViewModel);

        }

        //Helper method to Get order details using order id
        private OrderDetailsViewModel GetOrderDetails(int orderId)
        {
            var orderDetailsVM = context.orders
                .Where(o => o.order_id == orderId)
                .Select(ovm => new OrderDetailsViewModel
                {
                    order_id = ovm.order_id,
                    order_status = ovm.order_status,
                    order_date = ovm.order_date,
                    required_date = ovm.required_date,
                    shipped_date = ovm.shipped_date,
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
                        list_price = oi.list_price,
                        discount = oi.discount,
                        store_id = oi.store_id,
                        store_name = oi.store.store_name,
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

        public ActionResult OrderSummary(int pId = 0)
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

        [Authorize]
        [HttpPost]
        public ActionResult PlaceOrder(int pId = 0)
        {
            order newOrder = new order();
            int orderId;
            
            decimal discountPercentage = 15m / 100m;

            //get the logged in user id
            var user = context.users
                            .Where(x => x.email == User.Identity.Name)                            
                            .SingleOrDefault();

            // Check if user address is available
            if (user == null || string.IsNullOrEmpty(user.street) || string.IsNullOrEmpty(user.city) ||
                string.IsNullOrEmpty(user.state) || string.IsNullOrEmpty(user.zip_code))
            {
                ViewBag.ErrorMsg = "Address information is incomplete. Please update your address before placing an order.";
                return View("Error");
            }
            //For single product order --------------
            if (pId > 0)
            {
                //Get product stock details
                stock itemStock = context.stocks
                       .Where(z => z.product_id == pId)
                       .FirstOrDefault();

                //Check if product is in stock
                if (itemStock.quantity > 0)
                {
                    var product = context.products
                    .FirstOrDefault(p => p.product_id == pId);

                    newOrder = new order()
                    {
                        customer_id = user.user_id,
                        order_status = (byte)OrderStatus.Placed,
                        order_date = DateTime.Now,
                        required_date = DateTime.Now.AddDays(7),
                    };
                    context.orders.Add(newOrder);
                    context.SaveChanges();

                    orderId = newOrder.order_id;
                    if (orderId > 0)
                    {

                        order_items item = new order_items()
                        {
                            order_id = orderId,
                            product_id = pId,
                            quantity = 1,
                            list_price = product.list_price - (product.list_price * discountPercentage),
                            discount = discountPercentage,
                            store_id = itemStock.store_id,
                           
                        };
                        context.order_items.Add(item);

                        //decrease the stock
                        var stock = context.stocks
                            .Where(p => p.product_id == pId)
                            .FirstOrDefault();
                        stock.quantity = stock.quantity - 1;

                        context.SaveChanges();
                        return RedirectToAction("OrderSuccess");
                    }
                }
                return View("Error");

            }

            //when order through cart ----------------
            // Get the user's cart items
            var productIds = context.userCarts
                .Where(c => c.user_id == user.user_id)
                .Select(p => p.product_id)
                .ToList();

            //Get product stock details
            List<stock> itemStockList = context.stocks
                   .Where(z=>productIds.Contains(z.product_id))
                   .ToList();

            //check if all products are in stock
            bool isInStock = itemStockList.All(x => x.quantity > 0);

            if (productIds.Count > 0 && isInStock)
            {
                newOrder = new order()
                {
                    customer_id = user.user_id,
                    order_status = (byte)OrderStatus.Placed,
                    order_date = DateTime.Now,
                    required_date = DateTime.Now.AddDays(7),
                };

                context.orders.Add(newOrder);
                context.SaveChanges();

                orderId = newOrder.order_id;

                if (orderId > 0)
                {
                    
                    var cartProducts = context.products
                                              .Where(p => productIds.Contains(p.product_id))
                                              .ToList();

                    foreach (var product in cartProducts)
                    {
                        var stockInfo = itemStockList
                                .FirstOrDefault(x => x.product_id == product.product_id);

                        order_items item = new order_items()
                        {
                            order_id = orderId,
                            product_id = product.product_id,
                            quantity = 1,
                            list_price = product.list_price - (product.list_price * discountPercentage),
                            discount = discountPercentage,
                            store_id = stockInfo != null ? stockInfo.store_id : 0
                        };

                        context.order_items.Add(item);
                        //decrease the stocks for each items
                        foreach (var productId in productIds)
                        {
                            var stock = context.stocks
                                .Where(s => s.product_id == productId)
                                .FirstOrDefault();

                            stock.quantity = stock.quantity - 1;
                        }

                        //Empty the cart
                        userCart cart = context.userCarts
                                .FirstOrDefault(c => c.user_id == user.user_id && c.product_id == product.product_id);
                        context.userCarts.Remove(cart);

                        context.SaveChanges();
                    }
                    return RedirectToAction("OrderSuccess");
                }
            }

            return View("Error");
        }

        public ActionResult OrderSuccess()
        {
            return View();
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

        //Redirect user to order success page if order confirmed
       

        public ActionResult _DownloadOrderSummary(int orderId)
        {
            if (orderId < 0)
            {
                return View("Error");
            }
            SummaryPDFmodel orderSummary = context.orders
                .Where(o => o.order_id == orderId)
                .Select(x => new SummaryPDFmodel
                {
                    orderId = x.order_id,
                    downloadDate = DateTime.Now,
                    orderDate = x.order_date,
                    orderTotal = x.order_items.Sum(oi => oi.list_price),
                    item = x.order_items
                        .Where(oi => oi.order_id == orderId)
                        .Select(oi => new OrderItem
                        {
                            list_price = oi.list_price,
                            discount = oi.discount,
                            item_id = oi.item_id,
                            quantity = oi.quantity,
                            product_id = oi.product_id,
                            store_name = oi.store.store_name,
                            productDetails = new ProductDetailsViewModel
                            {
                                product_name = oi.product.product_name,
                                list_price = oi.product.list_price
                            }
                        }).ToList(),
                    userDetails = new UserViewModel
                    {
                        first_name = x.user.first_name,
                        street = x.user.street,
                        city = x.user.city,
                        state = x.user.state,
                        zip_code = x.user.zip_code,
                        email = x.user.email,
                        phone = x.user.phone,
                        user_id = x.user.user_id,
                    }
                }).FirstOrDefault();

            return new ViewAsPdf("_DownloadOrderSummary", orderSummary)
            {
                FileName = $"Order_{orderId}.pdf",
                PageSize = Rotativa.Options.Size.A4,
                CustomSwitches = "--disable-smart-shrinking"
            };
        }
        public ActionResult GetLogoImage()
        {
            string imagePath = Server.MapPath("~/Images/Utilities/logo.png");
            return File(imagePath, "image/png");
        }
    }
}