using Project_MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Project_MVC.Services
{
    interface IOrderService
    {
        IEnumerable<Order> GetList();
        int? Create(Order item);
        int? UpdateStatus(Order item);
        Order Detail(int? id);
        bool Update(Order existItem, Order item, ModelStateDictionary state);
        bool Delete(Order item, ModelStateDictionary state);
        void DisposeDb();
    }
}
