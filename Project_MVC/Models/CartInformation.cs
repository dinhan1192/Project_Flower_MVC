using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Project_MVC.Models
{
    public class CartInformation
    {
        public int? Id { get; set; }
        [Required]
        public string ShipName { get; set; }
        [Required]
        public string ShipPhone { get; set; }
        [Required]
        public string ShipAddress { get; set; }
        public string PaymentTypeId { get; set; }
    }
}