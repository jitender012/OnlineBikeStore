﻿using System.Collections.Generic;
using System.Web;

namespace OnlineBikeStore.Models
{
    public class ProductViewModel
    {
        public int product_id { get; set; }
        public string product_name { get; set; }
        public int brand_id { get; set; }
        public int category_id { get; set; }
        public short model_year { get; set; }
        public decimal list_price { get; set; }
        public string description { get; set; }
        public string url { get; set; }
        public string product_type { get; set; }       
        public HttpPostedFileBase ImageFile { get; set; }
        public bool isInWishList { get; set; }
    }

    public class ProductDetailsViewModel
    {
        public int product_id { get; set; }
        public string product_name { get; set; }
        public int brand_id { get; set; }
        public string brand_name { get; set; }
        public int category_id { get; set; }
        public string category_name { get; set; }
        public short model_year { get; set; }
        public decimal list_price { get; set; }
        public string description { get; set; }
        public string url { get; set; }
        public string product_type { get; set; }
        public bool isInCart { get; set; }
        public bool isInWishList { get; set; }
        public bool isPurchased { get; set; }
        public bool isReviewed { get; set; }
        public List<FeedbackViewModel> Feedback { get; set; }
        public HttpPostedFileBase ImageFile { get; set; }
        public int currentStock { get; set; }
    }
}