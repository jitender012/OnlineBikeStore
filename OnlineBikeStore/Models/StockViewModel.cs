namespace OnlineBikeStore.Models
{
    public class StockViewModel
    {
        public string product_name { get; set; }
        public string product_image { get; set; }
        public short model_year { get; set; }
        public int store_id { get; set; }
        public int product_id { get; set; }
        public int? quantity { get; set; }
    }   
  
}