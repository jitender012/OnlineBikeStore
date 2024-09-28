using System;
using System.Collections.Generic;

namespace OnlineBikeStore.Models
{
    public class OrderViewModel
    {
        public int? customer_id { get; set; }
        public int order_id { get; set; }        
        public byte order_status { get; set; }
        public DateTime order_date { get; set; }
        public DateTime required_date { get; set; }
        public DateTime? shipped_date { get; set; }
        public decimal? total_price { get; set; }
        public List<string> item_names {  get; set; }
        public string first_product_image { get; set; }
        public int total_items { get; set; }
              
    }
   
    public class OrderDetailsViewModel
    {
        public int order_id { get; set; }             
        public byte order_status { get; set; }
        public DateTime order_date { get; set; }
        public DateTime required_date { get; set; }       
        public DateTime? shipped_date { get; set; }       
        public UserViewModel userDetails { get; set; }
        public List<OrderItem> orderItems { get; set; }
    }

    public class OrderItem
    {
        public int item_id { get; set; }
        public int order_id { get; set; }
        public int product_id { get; set; }
        public int? store_id { get; set; }
        public string store_name{ get; set; }
        public int quantity { get; set; }
        public decimal list_price { get; set; }
        public decimal discount { get; set; }
        public ProductDetailsViewModel productDetails { get; set; }
    }
    public class OrderSummaryViewModel
    {
        public UserViewModel user { get; set; }
        public List<ProductViewModel> products { get; set; }
    }
    public class SummaryPDFmodel
    {
        public int orderId { get; set; }
        public DateTime orderDate { get; set; }
        public DateTime downloadDate { get; set; }
        public decimal orderTotal { get; set; }
        public List<OrderItem> item { get;set; }
        public UserViewModel userDetails { get; set; }
    }
    public enum OrderStatus
    {
        Placed = 0,
        Shipped = 1,
        Delivered = 2,
        Cancelled = 3     
    }
}