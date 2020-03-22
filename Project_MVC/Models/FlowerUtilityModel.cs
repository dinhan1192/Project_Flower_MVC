using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Project_MVC.Models
{
    public class HeaderBarModel
    {
        public int Count { get; set; }
        public string CategoryCode { get; set; }
    }

    public class DateTimeModel
    {
        public DateTime? startDate { get; set; }
        public DateTime? endDate { get; set; }
    }

    public class RevenueModel
    {
        public string RevenueOf { get; set; }
        public double TotalRevenue { get; set; }
    }

    public class FlowersInOrderModel
    {
        [Key]
        public int? Id { get; set; }
        public string FlowerName { get; set; }
        public string ImageUrl { get; set; }
        public int Quantity { get; set; }
        public double TotalPricePerFlower { get; set; }
    }
}