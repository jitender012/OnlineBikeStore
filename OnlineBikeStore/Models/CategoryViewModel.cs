using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OnlineBikeStore.Models
{
    public class CategoryViewModel
    {
        [Key]
        public int category_id { get; set; }
        public string category_name { get; set; }
        public string category_image { get; set; }
        public HttpPostedFileBase ImageFile { get; set; }
    }
}