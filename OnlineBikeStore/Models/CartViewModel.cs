using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineBikeStore.Models
{
    public class CartViewModel
    {
        public int cart_id { get; set; }
        public int user_id { get; set; }
        public int product_id { get; set; }

    }
}