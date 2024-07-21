using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineBikeStore.Models
{
    public class CartViewModel
    {        
        public UserViewModel user { get; set; }
        public List<ProductViewModel> products { get; set; }
    }
}