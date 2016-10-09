using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WPFPro.Models
{
    public class Contactus
    {        
        public string contactName { get; set; }
        public string contactEmail { get; set; }
        public string mobilePhone { get; set; }
        public string note { get; set; }
        public DateTime createtime { get; set; }
    }
}