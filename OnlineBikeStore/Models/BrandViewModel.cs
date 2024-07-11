using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineBikeStore.Models
{
    public class BrandViewModel
    {
        public int brand_id { get; set; }
        public string brand_name { get; set; }
        public string brand_image { get; set; }
        public HttpPostedFileBase ImageFile { get; set; }
    }
}