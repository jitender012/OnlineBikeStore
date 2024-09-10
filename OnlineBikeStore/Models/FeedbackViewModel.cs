using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace OnlineBikeStore.Models
{
    public class FeedbackViewModel
    {
        public int feedback_id { get; set; }        
        public int customer_id { get; set; }
        public string customer_name { get; set; }
        [Required]
        public int product_id { get; set; }
        public DateTime date { get; set; }
        public string image_url { get; set; }
        public string feedback_text { get; set; }

        
        [Required(ErrorMessage = "Please select a rating.")]
        public int ratingValue { get; set; }
        public string product_img {  get; set; }
        public string product_name { get; set; }
        public HttpPostedFileBase ImageFile { get; set; }
    }

}