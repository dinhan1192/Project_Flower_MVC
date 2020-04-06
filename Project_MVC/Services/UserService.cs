using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Project_MVC.App_Start;
using Project_MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;

namespace Project_MVC.Services
{
    public class UserService : IUserService
    {
        private MyDbContext _db;
        public MyDbContext DbContext
        {
            get { return _db ?? HttpContext.Current.GetOwinContext().Get<MyDbContext>(); }
            set { _db = value; }
        }

        private ApplicationUserManager _userManager;
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public string GetCurrentUserId()
        {
            var claimsIdentity = HttpContext.Current.User.Identity as ClaimsIdentity;
            if (claimsIdentity != null)
            {
                var userIdClaim = claimsIdentity.Claims
                    .FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);

                if (userIdClaim != null)
                {
                    return userIdClaim.Value;
                }
            }

            return "";
        }

        public string GetCurrentUserName()
        {
            return HttpContext.Current.User.Identity.Name;
        }

        public string GetUserNameByUserId(string id)
        {
            var user = DbContext.Users.Find(id);
            if (user == null)
            {
                return GetCurrentUserName();
            }
            return user.UserName;
        }
    }
}