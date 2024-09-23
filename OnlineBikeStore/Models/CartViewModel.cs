using System.Collections.Generic;

namespace OnlineBikeStore.Models
{
    public class CartViewModel
    {        
        public UserViewModel user { get; set; }
        public List<ProductViewModel> products { get; set; }
    }
}