using System;
using System.Collections.Generic;
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
        [ForeignKey("Flower")]
        public string FlowerCode { get; set; }
        [ForeignKey("Order")]
        public int? OrderId { get; set; }
        public virtual Order Order { get; set; }
        public virtual Flower Flower { get; set; }
        public int Quantity { get; set; }
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