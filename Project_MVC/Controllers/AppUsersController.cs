using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Project_MVC.App_Start;
using Project_MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Diagnostics;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Project_MVC.Controllers
{
    [Authorize(Roles = Constant.Admin)]
    public class AppUsersController : Controller
    {
        // GET: AppUsers
         private MyDbContext _db;
        public MyDbContext DbContext
        {
            get { return _db ?? HttpContext.GetOwinContext().Get<MyDbContext>(); }
            set { _db = value; }
        }
        private ApplicationUserManager _userManager;
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        //public AppUsersController()
        //{
        //    DbContext = new MyDbContext();
        //    UserStore<AppUser> userStore = new UserStore<AppUser>(DbContext);
        //    UserManager = new UserManager<AppUser>(userStore);
        //}

        [HttpGet]
        public PartialViewResult AddRolePopup(string Id)
        {
            ViewBag.userIds = Id;
            var roles = DbContext.IdentityRoles.ToList();
            return PartialView("PopupForAddRole", roles);
        }

        //public ActionResult AddUsers2Roles(string[] ids, string[] roleNames)
        //{
        //    foreach (var id in ids)
        //    {
        //        userManager.AddToRoles(id, roleNames);
        //    }
        //    return View();
        //}

        [HttpPost]
        public ActionResult AddUsers2Roles(string Id, string RoleName)
        {
            var arrUserIds = Id.Split(',');
            var arrRoleNames = RoleName.Split(',');
            foreach (var id in arrUserIds)
            {
                foreach (var roleName in arrRoleNames)
                {
                    UserManager.AddToRole(id, roleName);
                }
                //UserManager.AddToRoles(id, arrRoleNames);
            }

            //try
            //{
            //    var arrUserIds = Id.Split(',');
            //    var arrRoleNames = RoleName.Split(',');
            //    foreach (var id in arrUserIds)
            //    {
            //        //foreach(var roleName in arrRoleNames)
            //        //{
            //        //    UserManager.AddToRole(id, roleName);
            //        //}
            //        UserManager.AddToRoles(id, arrRoleNames);
            //    }
            //}
            //catch (Exception e)
            //{
            //    Debug.WriteLine(e);
            //    throw;
            //}

            return RedirectToAction("Index");
        }
        // GET: AppUsers
        public ActionResult Index(int? page)
        {
            var users = DbContext.Users.ToList();
            int pageSize = Constant.PageSize;
            int pageNumber = (page ?? 1);
            ThisPage thisPage = new ThisPage()
            {
                CurrentPage = pageNumber,
                TotalPage = Math.Ceiling((double)users.Count() / pageSize)
            };
            ViewBag.Page = thisPage;
            return View(users.Skip(pageSize * (pageNumber - 1)).Take(pageSize).ToList());
        }
    }
}