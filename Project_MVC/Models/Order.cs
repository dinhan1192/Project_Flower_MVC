using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project_MVC.Models
{
    public class Order
    {
        public int? Id { get; set; }
        public int MemberId { get; set; }
        public PaymentType PaymentTypeId { get; set; }
        public string ShipName { get; set; }
        public string ShipAddress { get; set; }
        public string ShipPhone { get; set; }
        public double TotalPrice { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public string DeletedBy { get; set; }
        public OrderStatus Status { get; set; }

        public enum OrderStatus
        {
            Pending = 6, condirmed = 5, Shipping = 4, Paid = 3, Done = 2, Cancel = 1, NotDeleted = 0, Deleted = -1
        }
        public enum PaymentType
        {
            Cod = 1, InternetBanking = 2, DirectTransfer = 3
        }
        public Order()
        {
            //CreatedAt = DateTime.Now;
            //UpdatedAt = DateTime.Now;
            Status = OrderStatus.Pending;
        }

        internal bool IsDeleted()
        {
            return this.Status == OrderStatus.Deleted;
        }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}