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
        IEnumerable<RevenueModel> GetListRevenues(string start, string end);
        IEnumerable<RevenuePieChartModel> GetListRevenuesForPieChart(string start, string end);
        int? UpdateStatus(Order item, string userName);
        Order Detail(int? id);
        bool Update(Order existItem, Order item, ModelStateDictionary state);
        bool Delete(Order item, ModelStateDictionary state);
        void DisposeDb();
    }
}
