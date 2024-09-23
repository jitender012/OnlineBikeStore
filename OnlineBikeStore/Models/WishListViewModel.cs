using System.ComponentModel.DataAnnotations;

namespace OnlineBikeStore.Models
{
    public class WishListViewModel
    {
        public int wl_item_id { get; set; }
        [Required]
        public int product_id { get; set; }
        public int user_id { get; set; }
        public string product_name { get; set; }
        public decimal price { get; set; }
        public string image_url { get; set; }
    }
}