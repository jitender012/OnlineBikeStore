//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace OnlineBikeStore
{
    using System;
    using System.Collections.Generic;
    
    public partial class user_role
    {
        public int role_id { get; set; }
        public int user_id { get; set; }
        public string role { get; set; }
    
        public virtual user user { get; set; }
    }
}