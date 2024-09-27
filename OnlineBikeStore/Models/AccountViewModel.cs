using System;
using System.ComponentModel.DataAnnotations;

namespace OnlineBikeStore.Models
{
    public class AccountViewModel
    {
        public int user_id { get; set; }

        [Required]
        [Display(Name = "First Name")]
        public string first_name { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string last_name { get; set; }

        [Required]
        [Display(Name = "Email ID")]
        public string email { get; set; }

        [Required]
        public string password { get; set; }
        public int role_id { get; set; }
        public string role { get; set; }  
    }
    public class LoginViewModel
    {
        [Required]
        public string email { get; set; }
        [Required]
        public string password { get; set; }
    }   
    
}