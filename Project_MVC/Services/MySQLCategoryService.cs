using Microsoft.AspNet.Identity.Owin;
using Project_MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Project_MVC.Services
{
    public class MySQLCategoryService : ICRUDService<Category>
    {
        private MyDbContext _db;
        public MyDbContext DbContext
        {
            get { return _db ?? HttpContext.Current.GetOwinContext().Get<MyDbContext>(); }
            set { _db = value; }
        }

        private IUserService userService;
        private IImageService mySQLImageService { get; set; }

        public MySQLCategoryService()
        {
            userService = new UserService();
            mySQLImageService = new MySQLImageService();
        }
        public bool Create(Category item, ModelStateDictionary state)
        {
            throw new NotImplementedException();
        }

        public bool CreateWithImage(Category item, ModelStateDictionary state, IEnumerable<HttpPostedFileBase> images, IEnumerable<HttpPostedFileBase> videos)
        {
            Validate(item, state);
            if (state.IsValid)
            {
                //product.ProductCategoryId = Utils.Utility.GetNullableInt(product.ProductCategoryNameAndId.Split(' ')[0]);
                //product.ProductCategoryName = product.ProductCategoryNameAndId.Substring(product.ProductCategoryNameAndId.IndexOf('-') + 2);
                item.CreatedAt = DateTime.Now;
                item.UpdatedAt = DateTime.Now;
                item.UpdatedBy = userService.GetCurrentUserName();
                item.DeletedAt = null;
                item.CreatedBy = userService.GetCurrentUserName();
                item.Status = Category.CategoryStatus.NotDeleted;
                DbContext.Categories.Add(item);
                // add image to table ProductImages
                var lstImages = mySQLImageService.SaveImage2List(item.Code, Constant.CategoryImage, images);
                foreach (var image in lstImages)
                {
                    item.ImageData = image.ImageData;
                    break;
                }
                //item.ProductVideos = mySQLImageService.SaveVideo2List(item.Code, videos);
                //
                DbContext.SaveChanges();
                return true;

            }

            //ViewBag.ProductCategoryId = new SelectList(db.ProductCategories, "Id", "Name", product.ProductCategoryId);
            return false;
        }

        public bool Delete(Category item, ModelStateDictionary state)
        {
            throw new NotImplementedException();
        }

        public Category Detail(string id)
        {
            throw new NotImplementedException();
        }

        public void DisposeDb()
        {
            DbContext.Dispose();
        }

        public IEnumerable<Category> GetList()
        {
            return DbContext.Categories.Where(s => s.Status == Category.CategoryStatus.NotDeleted).ToList();
        }

        public bool Update(Category existItem, Category item, ModelStateDictionary state)
        {
            throw new NotImplementedException();
        }

        public bool UpdateWithImage(Category existItem, Category item, ModelStateDictionary state, IEnumerable<HttpPostedFileBase> images)
        {
            throw new NotImplementedException();
        }

        public void Validate(Category item, ModelStateDictionary state)
        {
            if (string.IsNullOrEmpty(item.Code))
            {
                state.AddModelError("Code", "Product Category Code is required.");
            }
            var list = DbContext.Categories.Where(s => s.Code == item.Code).ToList();
            if (list.Count != 0)
            {
                state.AddModelError("Code", "Product Category Code already exist.");
            }
        }

        public void ValidateCategory(Category item, ModelStateDictionary state)
        {
            throw new NotImplementedException();
        }

        public bool ValidateStringCode(string code)
        {
            throw new NotImplementedException();
        }
    }
}