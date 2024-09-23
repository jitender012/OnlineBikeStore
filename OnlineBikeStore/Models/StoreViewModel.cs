using System.Collections.Generic;

namespace OnlineBikeStore.Models
{
    public class StoreViewModel
    {
        public int store_id { get; set; }
        public string store_name { get; set; }
        public string phone { get; set; }
        public string email { get; set; }
        public string street { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string zip_code { get; set; }
    }
    public class StoreDashboardViewModel
    {
        public int store_id { get; set; }
        public string store_name { get; set; }
        public List<StockViewModel> stock { get; set; }
    }
}