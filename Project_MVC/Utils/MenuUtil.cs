using Microsoft.AspNet.Identity.Owin;
using Project_MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Principal;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Project_MVC.Utils
{
    public class MenuUtil
    {
        private static Assembly asm;
        private static MyDbContext _db;
        public static MyDbContext DbContext
        {
            get { return _db ?? HttpContext.Current.GetOwinContext().Get<MyDbContext>(); }
            set { _db = value; }
        }
        private static List<ProductCategory> _listLevelOneProductCategories;
        private static List<ProductCategory> _listProductCategories;

        private static List<LevelOneMenu> _listLevelOneMenus;
        private static List<LevelTwoMenu> _listLevelTwoMenus;

        public MenuUtil()
        {
            asm = Assembly.GetAssembly(typeof(Project_MVC.MvcApplication));
        }

        public static List<ProductCategory> GetLevelOneProductCategories()
        {
            _listLevelOneProductCategories = DbContext.ProductCategories.ToList();
            _listLevelOneProductCategories = _listLevelOneProductCategories.Where(s => string.IsNullOrEmpty(s.LevelOneProductCategoryCode) && s.Status == ProductCategory.ProductCategoryStatus.NotDeleted).ToList();
            return _listLevelOneProductCategories;
        }

        public static void SetLevelOneProductCategories(List<ProductCategory> lstLevelOneProductCategories)
        {
            _listLevelOneProductCategories = lstLevelOneProductCategories;
        }

        public static List<ProductCategory> GetProductCategories(string Code)
        {
            //if (_listProductCategories == null || _listProductCategories.Count == 0)
            //{
            //    _listProductCategories = DbContext.ProductCategories.Where(s => s.LevelOneProductCategory.Code == Code).ToList();
            //}
            if (string.IsNullOrEmpty(Code))
            {
                _listProductCategories = DbContext.ProductCategories.ToList();
            }
            else
            {
                _listProductCategories = DbContext.ProductCategories.Where(s => s.LevelOneProductCategoryCode == Code && s.Status == ProductCategory.ProductCategoryStatus.NotDeleted).ToList();
            }
            return _listProductCategories;
        }

        public static void SetProductCategories(List<ProductCategory> lstProductCategories)
        {
            _listProductCategories = lstProductCategories;
        }

        //public static List<SelectListItem> GetCategoriesAsDropDownList()
        //{
        //    List<SelectListItem> list = new List<SelectListItem>();
        //    if (_listCategories == null)
        //    {
        //        _listCategories = db.Categories.ToList();
        //    }

        //    foreach (var category in _listCategories)
        //    {
        //        list.Add(new SelectListItem { Text = category.Name, Value = category.Id.ToString() });
        //    }
        //    return list;
        //}

        #region Menu

        public static List<LevelOneMenu> GetLevelOneMenus()
        {
            _listLevelOneMenus = DbContext.LevelOneMenus.ToList();
            return _listLevelOneMenus;
        }

        public static List<LevelTwoMenu> GetLevelTwoMenus(string Code)
        {
            //if (_listProductCategories == null || _listProductCategories.Count == 0)
            //{
            //    _listProductCategories = DbContext.ProductCategories.Where(s => s.LevelOneProductCategory.Code == Code).ToList();
            //}
            _listLevelTwoMenus = DbContext.LevelTwoMenus.Where(s => s.LevelOneMenu.Code == Code).ToList();
            return _listLevelTwoMenus;
        }

        public static List<Type> GetControllerNames()
        {
            //if (_listProductCategories == null || _listProductCategories.Count == 0)
            //{
            //    _listProductCategories = DbContext.ProductCategories.Where(s => s.LevelOneProductCategory.Code == Code).ToList();
            //}
            var controllerList = Assembly.GetAssembly(typeof(Project_MVC.MvcApplication)).GetTypes().Where(type => typeof(System.Web.Mvc.Controller).IsAssignableFrom(type)).ToList();
            return controllerList;
        }

        public static List<MethodInfo> GetActionNames(string name)
        {
            var controllerName = name + "Controller";
            var actionList = GetControllerNames().Where(type => type.Name == controllerName).FirstOrDefault().GetMethods()
    .Where(method => method.IsPublic && !method.IsDefined(typeof(NonActionAttribute))).ToList();
            return actionList;
        }

        public static List<SelectListItem> GetControllersAsDropDownList()
        {
            List<SelectListItem> list = new List<SelectListItem>();

            foreach (var controller in GetControllerNames())
            {
                var controlerName = controller.Name.Remove(controller.Name.Length - 10, 10);
                list.Add(new SelectListItem { Text = controlerName, Value = controlerName });
            }
            return list;
        }

        //public static List<SelectListItem> GetActionsAsDropDownList()
        //{
        //    List<SelectListItem> list = new List<SelectListItem>();

        //    foreach (var action in GetActionNames())
        //    {
        //        list.Add(new SelectListItem { Text = action.Name, Value = action.Name });
        //    }
        //    return list;
        //}

        #endregion
    }

    public static class RolesUtil
    {
        public static bool IsInAnyRole(this IPrincipal user, string[] roles)
        {
            //Check if authenticated first (optional)
            if (!user.Identity.IsAuthenticated) return false;
            //foreach(var role in roles)
            //{
            //    if (user.IsInRole(role))
            //    {
            //        return true;
            //    }
            //}
            //return false;
            return roles.Any(user.IsInRole);
        }
    }
}