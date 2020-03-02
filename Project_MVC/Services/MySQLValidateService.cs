using Microsoft.AspNet.Identity.Owin;
using Project_MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project_MVC.Services
{
    public class MySQLValidateService : IValidateService
    {
        private MyDbContext _db;
        public MyDbContext DbContext
        {
            get { return _db ?? HttpContext.Current.GetOwinContext().Get<MyDbContext>(); }
            set { _db = value; }
        }
        public bool ValidateUserProducts(string code, string id)
        {
            var list = DbContext.UserProducts.Where(s => s.ProductCode.Contains(code) && s.UserId.Contains(id)).ToList();
            if (list.Count != 0)
            {
                return true;
            }

            return false;
        }
    }
}