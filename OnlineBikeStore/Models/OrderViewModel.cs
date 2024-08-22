using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineBikeStore.Models
{
    public class OrderViewModel
    {
        public int order_id { get; set; }        
        public byte order_status { get; set; }
        public DateTime order_date { get; set; }
        public DateTime required_date { get; set; }
        public DateTime? shipped_date { get; set; }
        public decimal total_price { get; set; }
        
    }
    public class OrderDetailsViewModel
    {
        public int order_id { get; set; }             
        public byte order_status { get; set; }
        public DateTime order_date { get; set; }
        public DateTime required_date { get; set; }       
        public UserViewModel userDetails { get; set; }
        public List<OrderItem> orderItems { get; set; }
    }

    public class OrderItem
    {
        public int item_id { get; set; }        
        public int quantity { get; set; }
        public ProductDetailsViewModel productDetails { get; set; }
    }
    public class OrderSummaryViewModel
    {
        public UserViewModel user { get; set; }
        public List<ProductViewModel> products { get; set; }
    }
    public enum OrderStatus
    {
        Placed = 0,
        Shipped = 1,
        Delivered = 2,
        Cancelled = 3,
        Returned = 4
    }
}