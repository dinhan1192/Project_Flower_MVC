using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Project_MVC.Models
{
    public static class Constant
    {
        public const int PageSize = 3;
        public const int PageSizeOnCustomerPage = 12;
        public const int PageVideoSize = 1;
        public const string User = "User";
        public const string Admin = "Admin";
        public const string Admin01 = "Admin01";
        public const string Employee = "Employee";
        public const int EmailNotConfirmed = 0; 
        public const int EmailConfirmed = 1;
        public const int FirstDisplayOrder = 1;
        public const string Customer = "Customer";
        public const string CreateRating = "CreateRating";
        public const string UpdateRating = "UpdateRating";
        public const string CreateProduct = "CreateProduct";
        
        #region Images

        public const int ProductImage = 1;
        public const int OwnerOfCourseImage = 2;

        #endregion

        public static List<SelectListItem> ListActionName = new List<SelectListItem>()
        {
            new SelectListItem{ Text= "Index", Value = "1" },
            new SelectListItem{ Text= "Create", Value = "2" },
            new SelectListItem{ Text= "Edit", Value = "3" },
            new SelectListItem{ Text= "Detail", Value = "4" },
            new SelectListItem{ Text= "Delete", Value = "5" },
        };
    }
}