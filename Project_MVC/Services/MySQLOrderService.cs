using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.Owin;
using Project_MVC.Models;
using static Project_MVC.Models.Order;

namespace Project_MVC.Services
{
    public class MySQLOrderService : IOrderService
    {
        private MyDbContext _db;
        public MyDbContext DbContext
        {
            get { return _db ?? HttpContext.Current.GetOwinContext().Get<MyDbContext>(); }
            set { _db = value; }
        }

        private IUserService userService;

        public MySQLOrderService()
        {
            userService = new UserService();
        }

        //public int? Create(Order item)
        //{
        //    var existOrder = DbContext.Orders.Where(s => s.ShipName == item.ShipName).ToList();
        //    var code = item.OrderDetails.FirstOrDefault().FlowerCode;
        //    var existOrderDetail = DbContext.OrderDetails.Where(s => s.FlowerCode == code).ToList();
        //    if (existOrder != null && existOrder.Count > 0 && existOrderDetail != null && existOrderDetail.Count > 0)
        //        return existOrderDetail.FirstOrDefault().OrderId;

        //    item.CreatedAt = DateTime.Now;
        //    item.UpdatedAt = null;
        //    item.DeletedAt = null;
        //    item.CreatedBy = userService.GetCurrentUserName();
        //    item.Status = OrderStatus.NotDeleted;
        //    DbContext.Orders.Add(item);
        //    DbContext.SaveChanges();

        //    return item.Id;
        //}

        //public bool CreateWithImage(Order item, ModelStateDictionary state, IEnumerable<HttpPostedFileBase> images, IEnumerable<HttpPostedFileBase> videos)
        //{
        //    throw new NotImplementedException();
        //}

        public bool Delete(Order item, ModelStateDictionary state)
        {
            if (state.IsValid)
            {
                item.Status = OrderStatus.Deleted;
                item.DeletedAt = DateTime.Now;
                item.DeletedBy = userService.GetCurrentUserName();
                DbContext.Orders.AddOrUpdate(item);
                DbContext.SaveChanges();

                return true;
            }

            return false;
        }

        public Order Detail(int? id)
        {
            return DbContext.Orders.Find(id);
        }

        public void DisposeDb()
        {
            DbContext.Dispose();
        }

        public IEnumerable<Order> GetList()
        {
            return DbContext.Orders.Where(s => s.Status != OrderStatus.Deleted);
        }

        public int? UpdateStatus(Order item)
        {
            item.UpdatedAt = null;
            item.UpdatedBy = userService.GetCurrentUserName();
            item.Status = OrderStatus.Paid;
            DbContext.Orders.AddOrUpdate(item);
            DbContext.SaveChanges();

            return item.Id;
        }

        public bool UpdateWithImage(Order existItem, Order item, ModelStateDictionary state, IEnumerable<HttpPostedFileBase> images)
        {
            throw new NotImplementedException();
        }
        public void Validate(Order item, ModelStateDictionary state)
        {
            throw new NotImplementedException();
        }

        public void ValidateCategory(Order item, ModelStateDictionary state)
        {
            throw new NotImplementedException();
        }

        public bool ValidateStringCode(string code)
        {
            throw new NotImplementedException();
        }

        public bool Update(Order existItem, Order item, ModelStateDictionary state)
        {
            if (state.IsValid)
            {
                existItem.ShipName = item.ShipName;
                existItem.ShipPhone = item.ShipPhone;
                existItem.ShipAddress = item.ShipAddress;
                existItem.PaymentTypeId = item.PaymentTypeId;
                existItem.Status = item.Status;
                existItem.UpdatedAt = DateTime.Now;
                existItem.UpdatedBy = userService.GetCurrentUserName();
                DbContext.Orders.AddOrUpdate(existItem);
                DbContext.SaveChanges();

                return true;
            }

            return false;
            //throw new NotImplementedException();
        }

        public IEnumerable<RevenueModel> GetListRevenuesMonth(string year)
        {
            var intYear = DateTime.Now.Year;
            if (!string.IsNullOrEmpty(year))
            {
                intYear = Convert.ToInt32(year);
            }
            var lstOrder = DbContext.Orders.Where(s => (s.Status == OrderStatus.Paid || s.Status == OrderStatus.Done) && s.UpdatedAt.Value.Year == intYear).ToList();
            var lstRevenuesMonth = new List<RevenueModel>();
            if (lstOrder != null && lstOrder.Count > 0)
            {
                for (var i = Constant.FirstMonthOfYear; i <= Constant.EndMonthOfYear; i++)
                {
                    lstRevenuesMonth.Add(new RevenueModel()
                    {
                        RevenueOf = string.Format("{0}", i),
                        TotalRevenue = lstOrder.Where(s => s.UpdatedAt.Value.Month == i).Sum(s => s.TotalPrice)
                    });
                }
            }

            return lstRevenuesMonth;
        }

        public IEnumerable<RevenueModel> GetListRevenuesYear()
        {
            var thisYear = DateTime.Now.Year;
            var lstOrder = DbContext.Orders.Where(s => s.Status == OrderStatus.Paid || s.Status == OrderStatus.Done && s.UpdatedAt.Value.Year >= thisYear - 4).ToList();
            var lstRevenuesYear = new List<RevenueModel>();
            if (lstOrder != null && lstOrder.ToList().Count > 0)
            {
                for (var i = thisYear - 4; i <= thisYear; i++)
                {
                    lstRevenuesYear.Add(new RevenueModel()
                    {
                        RevenueOf = string.Format("{0}", i),
                        TotalRevenue = lstOrder.Where(s => s.UpdatedAt.Value.Year == i).Sum(s => s.TotalPrice)
                    });
                }
            }

            return lstRevenuesYear;
        }
    }
}
