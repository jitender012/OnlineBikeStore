﻿using AutoMapper;
using OnlineBikeStore.Extensions;
using OnlineBikeStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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
                }).ToList();

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

        [HttpGet]
        public PartialViewResult Orders(string orderId, string orderStatus)
        {
            var o_id = 0;
            var o_status = -1;

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

            if (!string.IsNullOrEmpty(orderId))
            {
                o_id = int.Parse(orderId);
            }


            if (User.Identity.IsAuthenticated)
            {
                var userId = context.GetUserId(User.Identity.Name);

                var ordersListVM = context.orders
                    .Select(z => new OrderViewModel
                    {
                        customer_id = z.customer_id.Value,
                        order_date = z.order_date,
                        order_id = z.order_id,
                        order_status = z.order_status,
                        required_date = z.required_date,
                        shipped_date = z.shipped_date,

                    }).ToList();

                //Search orders using order id
                if (o_id > 0)
                {
                    ordersListVM = ordersListVM
                        .Where(o => (o.order_id.ToString()).Contains(orderId))
                        .ToList();
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
                    return PartialView("_Orders", ordersListVM);
                }
                return PartialView("_Orders", ordersListVM);
            }
            return PartialView("Error");
        }

        [Authorize(Roles = "customer")]
        public ActionResult OrderDetailsCustomer(int orderId)
        {

            // Get user details
            var user = Mapper.Map<UserViewModel>(context.users
                            .Where(x => x.email == User.Identity.Name)
                            .SingleOrDefault());

            // Get the order details
            var orderDetails = context.orders
                            .Where(o => o.order_id == orderId)
                            .SingleOrDefault();

            // Get each order items in the order
            List<order_items> orderItems = context.order_items
                            .Where(x => x.order_id == orderId)
                            .ToList();

            var productIds = orderItems.Select(y => y.product_id).ToList();


            // Get details of each product in order items
            var products = context.products
                            .Where(x => productIds.Contains(x.product_id))
                            .ToList();

            //Map each order items retrieved from database to the order items view model
            var orderItemsVM = orderItems.Select(oi => new OrderItem
            {
                item_id = oi.item_id,
                quantity = oi.quantity,
                productDetails = products
                            .Where(p => p.product_id == oi.product_id)
                            .Select(p => new ProductDetailsViewModel
                            {
                                product_id = p.product_id,
                                product_name = p.product_name,
                                list_price = p.list_price,
                                url = p.url,
                                description = p.description
                            })
                            .SingleOrDefault()
            }).ToList();

            //Map all retrieved details to order details view model
            OrderDetailsViewModel orderDetailsViewModel = new OrderDetailsViewModel()
            {
                order_id = orderDetails.order_id,
                order_status = orderDetails.order_status,
                order_date = orderDetails.order_date,
                required_date = orderDetails.required_date,
                userDetails = user,
                orderItems = orderItemsVM
            };
            return View(orderDetailsViewModel);

        }

        [Authorize(Roles = "admin")]
        public ActionResult OrderDetailsAdmin(int orderId)
        {
            // Get user details
            var user = Mapper.Map<UserViewModel>(context.users
                            .Where(x => x.email == User.Identity.Name)
                            .SingleOrDefault());

            // Get the order details
            var orderDetails = context.orders
                            .Where(o => o.order_id == orderId)
                            .SingleOrDefault();

            // Get each order items in the order
            List<order_items> orderItems = context.order_items
                            .Where(x => x.order_id == orderId)
                            .ToList();

            var productIds = orderItems.Select(y => y.product_id).ToList();


            // Get details of each product in order items
            var products = context.products
                            .Where(x => productIds.Contains(x.product_id))
                            .ToList();

            //Map each order items retrieved from database to the order items view model
            var orderItemsVM = orderItems.Select(oi => new OrderItem
            {
                item_id = oi.item_id,
                quantity = oi.quantity,
                productDetails = products
                            .Where(p => p.product_id == oi.product_id)
                            .Select(p => new ProductDetailsViewModel
                            {
                                product_id = p.product_id,
                                product_name = p.product_name,
                                list_price = p.list_price,
                                url = p.url,
                                description = p.description
                            })
                            .SingleOrDefault()
            }).ToList();

            //Map all retrieved details to order details view model
            OrderDetailsViewModel orderDetailsViewModel = new OrderDetailsViewModel()
            {
                order_id = orderDetails.order_id,
                order_status = orderDetails.order_status,
                order_date = orderDetails.order_date,
                required_date = orderDetails.required_date,
                userDetails = user,
                orderItems = orderItemsVM
            };
            return View(orderDetailsViewModel);

        }

        public ActionResult UpdateOrderStatus(int orderId)
        {
            var order = context.orders
                .SingleOrDefault(o => o.order_id == orderId);

            if (order == null)
            {
                return HttpNotFound();
            }
            var orderDetails = new OrderViewModel
            {
                order_id = order.order_id,
                order_status = order.order_status,
                order_date = order.order_date
            };
            return View(orderDetails);
        }
        public ActionResult UpdateOrderStatus(OrderViewModel model)
        {
            if (model.order_id > 0)
            {
                var order = context.orders
                                .SingleOrDefault(o => o.order_id == model.order_id);
                if (order == null)
                {
                    return HttpNotFound();
                }
                order.order_status = model.order_status;
                context.SaveChanges();

                return RedirectToAction("OrderDetails", new { id = model.order_id });
            }
            return View(model);
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

                        return View("OrderSuccess");
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
                            userCart cart = context.userCarts.FirstOrDefault(c => c.user_id == userId && c.product_id == product.product_id);
                            context.userCarts.Remove(cart);
                            context.SaveChanges();
                        }
                        return View("OrderSuccess");
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