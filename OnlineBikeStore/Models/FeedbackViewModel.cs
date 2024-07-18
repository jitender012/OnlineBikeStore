using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineBikeStore.Models
{
    public class FeedbackViewModel
    {
        public int feedback_id { get; set; }
        public int customer_id { get; set; }
        public int product_id { get; set; }
        public DateTime date { get; set; }
        public string image_url { get; set; }
        public string feedback_text { get; set; }
        public int ratingValue { get; set; }
    }
}