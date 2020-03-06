using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Project_MVC.Models
{
    public class IdCount
    {
        [Key]
        public string Code { get; set; }
        public int Value { get; set; }
    }
}