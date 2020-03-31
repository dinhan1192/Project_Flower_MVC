using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Project_MVC.Models
{
    public class OrderDetail
    {
        // Khóa chính của OrderDetail là kết hợp của ProductId và OrderId
        [Key]
        public int? Id { get; set; }
        [DisplayName("Flower Code")]
        public string FlowerCode { get; set; }
        [DisplayName("Order Id")]
        public int? OrderId { get; set; }
        [ForeignKey("OrderId")]
        public virtual Order Order { get; set; }
        [ForeignKey("FlowerCode")]
        public virtual Flower Flower { get; set; }
        public int Quantity { get; set; }
        [DisplayName("Unit Price")]
        public double UnitPrice { get; set; }
        public OrderDetailStatus Status { get; set; }


        public enum OrderDetailStatus
        {
            NotDeleted = 0, Deleted = -1
        }

        internal bool IsDeleted()
        {
            return this.Status == OrderDetailStatus.Deleted;
        }
    }
}