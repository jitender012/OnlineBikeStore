﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineBikeStore.Models
{
    public class StockViewModel
    {
        public int store_id { get; set; }
        public int product_id { get; set; }
        public int? quantity { get; set; }
    }
    public class InventoryDomainModel
    {
        public string product_name { get; set; }
        public short model_year { get; set; }
        public int store_id { get; set; }
        public int product_id { get; set; }
        public int? quantity { get; set; }
    }
}