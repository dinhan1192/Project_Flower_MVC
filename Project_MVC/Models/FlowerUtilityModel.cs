using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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
        public DateTime TimeGetRevenue { get; set; }
        public string MonthYear { get; set; }
        public double TotalRevenue { get; set; }
    }

    public class RevenuePieChartModel
    {
        public string FlowerName { get; set; }
        public double FlowerRevenueRate { get; set; }
    }

    public class FlowersInOrderModel
    {
        [Key]
        public int? Id { get; set; }
        [DisplayName("Flower Name")]
        public string FlowerName { get; set; }
        [DisplayName("Image")] 
        public string ImageUrl { get; set; }
        public int Quantity { get; set; }
        [DisplayName("Total Price")]
        public double TotalPricePerFlower { get; set; }
        //[NotMapped]
        //[DisplayName("Order Id")]
        //public int? OrderId { get; set; }
        //[NotMapped]
        //[ForeignKey("OrderId")]
        //public virtual Order Order { get; set; }
    }
}