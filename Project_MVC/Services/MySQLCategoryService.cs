using Microsoft.AspNet.Identity.Owin;
using Project_MVC.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static Project_MVC.Models.Category;

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

                // add IdCounts
                var idCount = new IdCount()
                {
                    Code = item.Code,
                    Value = 0
                };
                DbContext.IdCounts.Add(idCount);
                DbContext.SaveChanges();
                return true;

            }

            //ViewBag.ProductCategoryId = new SelectList(db.ProductCategories, "Id", "Name", product.ProductCategoryId);
            return false;
        }

        public bool Delete(Category existCategory, ModelStateDictionary state)
        {
            if (state.IsValid)
            {
                existCategory.Status = CategoryStatus.Deleted;
                existCategory.DeletedAt = DateTime.Now;
                existCategory.DeletedBy = userService.GetCurrentUserName();
                DbContext.Categories.AddOrUpdate(existCategory);
                DbContext.SaveChanges();

                return true;
            }

            return false;
        }

        public Category Detail(string id)
        {
            return DbContext.Categories.Find(id);
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
            if (state.IsValid)
            {
                existItem.Name = item.Name;
                existItem.Description = item.Description;
                existItem.ParentNameAndCode = item.ParentNameAndCode;
                existItem.UpdatedAt = DateTime.Now;
                existItem.UpdatedBy = userService.GetCurrentUserName();
                DbContext.Categories.AddOrUpdate(existItem);
                DbContext.SaveChanges();

                return true;
            }

            return false;
            //throw new NotImplementedException();
        }

        public bool UpdateWithImage(Category existItem, Category item, ModelStateDictionary state, IEnumerable<HttpPostedFileBase> images)
        {
            ValidateCategory(item, state);
            if (state.IsValid)
            {
                existItem.Name = item.Name;
                existItem.ParentCode = item.ParentCode;
                existItem.Description = item.Description;
                existItem.UpdatedAt = DateTime.Now;
                existItem.UpdatedBy = userService.GetCurrentUserName();
                var lstImages = mySQLImageService.SaveImage2List(item.Code, Constant.CategoryImage, images);
                foreach (var image in lstImages)
                {
                    existItem.ImageData = image.ImageData;
                    break;
                }
                DbContext.FlowerImages.AddRange(lstImages);
                //
                DbContext.SaveChanges();
                return true;
            }

            return false;
        }

        public void Validate(Category item, ModelStateDictionary state)
        {
            if (string.IsNullOrEmpty(item.Code))
            {
                state.AddModelError("Code", "Flower Category Code is required.");
            }
            var list = DbContext.Categories.Where(s => s.Code == item.Code).ToList();
            if (list.Count != 0)
            {
                state.AddModelError("Code", "Flower Category Code already exist.");
            }
        }

        public void ValidateCategory(Category item, ModelStateDictionary state)
        {
            if (string.IsNullOrEmpty(item.ParentCode))
            {
                state.AddModelError("ParentNameAndCode", "Category is required.");
            }
        }

        public bool ValidateStringCode(string code)
        {
            throw new NotImplementedException();
        }
    }
}