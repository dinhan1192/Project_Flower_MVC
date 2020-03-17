using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Project_MVC.Models;

namespace Project_MVC.Services
{
    public class MySQLOrderService : IOrderService
    {
        public bool Delete(Order item, ModelStateDictionary state)
        {
            throw new NotImplementedException();
        }

        public Order Detail(int? id)
        {
            throw new NotImplementedException();
        }

        public void DisposeDb()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Order> GetList()
        {
            throw new NotImplementedException();
        }

        public bool Update(Order existItem, Order item, ModelStateDictionary state)
        {
            throw new NotImplementedException();
        }

        public int? UpdateStatus(Order item)
        {
            throw new NotImplementedException();
        }
    }
}