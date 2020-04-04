using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Project_MVC.Models
{
    public static class Constant
    {
        public const int PageSize = 10;
        public const int PageSizeOrdersOnCustomerPage = 18;
        public const int PageSizeFlowersOnCustomerPage = 9;
        public const int PageVideoSize = 1;
        public const int FirstMonthOfYear = 1;
        public const int EndMonthOfYear = 12;
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
        public const string ShoppingCart = "ShoppingCart";
        public const string CurrentOrder = "CurrentOrder";
        public const string DeleteUnknownOrders = "DeleteUnknownOrders";
        public const string SeedRandomOrders = "SeedRandomOrders";
        public const string WebURL = @"https://flowerattt.azurewebsites.net/";

        #region Images

        public const int FlowerImage = 1;
        public const int CategoryImage = 2;

        #endregion

        public static List<SelectListItem> ListActionName = new List<SelectListItem>()
        {
            new SelectListItem{ Text= "Index", Value = "1" },
            new SelectListItem{ Text= "Create", Value = "2" },
            new SelectListItem{ Text= "Edit", Value = "3" },
            new SelectListItem{ Text= "Detail", Value = "4" },
            new SelectListItem{ Text= "Delete", Value = "5" },
        };

        public static List<SelectListItem> ListSortFlower = new List<SelectListItem>()
        {
            new SelectListItem{ Text= "Name", Value = "1" },
            new SelectListItem{ Text= "Price", Value = "2" },
        };
    }
}