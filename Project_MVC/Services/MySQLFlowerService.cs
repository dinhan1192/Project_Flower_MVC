using Microsoft.AspNet.Identity.Owin;
using Project_MVC.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static Project_MVC.Models.Flower;

namespace Project_MVC.Services
{
    public class MySQLFlowerService : ICRUDService<Flower>
    {
        private MyDbContext _db;
        public MyDbContext DbContext
        {
            get { return _db ?? HttpContext.Current.GetOwinContext().Get<MyDbContext>(); }
            set { _db = value; }
        }

        private IUserService userService;
        private IImageService mySQLImageService { get; set; }

        public MySQLFlowerService()
        {
            userService = new UserService();
            mySQLImageService = new MySQLImageService();
        }
        public bool Create(Flower item, ModelStateDictionary state)
        {
            throw new NotImplementedException();
        }

        public bool CreateWithImage(Flower item, ModelStateDictionary state, IEnumerable<HttpPostedFileBase> images, IEnumerable<HttpPostedFileBase> videos)
        {
            // add IdCount
            var existIdCount = DbContext.IdCounts.Find(item.CategoryCode);
            existIdCount.Value++;
            DbContext.IdCounts.AddOrUpdate(existIdCount);
            item.Code = Utils.Utility.GenerateCode(existIdCount.Code, existIdCount.Value);
            Validate(item, state);
            ValidateCategory(item, state);
            if (state.IsValid)
            {
                //product.ProductCategoryId = Utils.Utility.GetNullableInt(product.ProductCategoryNameAndId.Split(' ')[0]);
                //product.ProductCategoryName = product.ProductCategoryNameAndId.Substring(product.ProductCategoryNameAndId.IndexOf('-') + 2);
                item.CreatedAt = DateTime.Now;
                item.UpdatedAt = DateTime.Now;
                item.UpdatedBy = userService.GetCurrentUserName();
                item.DeletedAt = null;
                item.CreatedBy = userService.GetCurrentUserName();
                item.Status = FlowerStatus.NotDeleted;
                DbContext.Flowers.Add(item);
                // add image to table ProductImages
                item.FlowerImages = mySQLImageService.SaveImage2List(item.Code, Constant.FlowerImage, images);
                //item.ProductVideos = mySQLImageService.SaveVideo2List(item.Code, videos);

                var existCategory = DbContext.Categories.Find(item.CategoryCode);
                var flowers = DbContext.Flowers.Where(s => s.CategoryCode == item.CategoryCode);
                var maxFlowerPrice = (double?)flowers.Max(s => s.Price);
                var minFlowerPrice = (double?)flowers.Min(s => s.Price);
                if (maxFlowerPrice == null)
                    maxFlowerPrice = 0;
                if (minFlowerPrice == null)
                    minFlowerPrice = 0;
                if (item.Price > maxFlowerPrice)
                {
                    existCategory.MaxPrice = item.Price;
                    existCategory.MinPrice = minFlowerPrice;
                }
                else if (item.Price < minFlowerPrice)
                {
                    existCategory.MaxPrice = maxFlowerPrice;
                    existCategory.MinPrice = item.Price;
                }
                else
                {
                    existCategory.MaxPrice = maxFlowerPrice;
                    existCategory.MinPrice = minFlowerPrice;
                }
                DbContext.SaveChanges();
                //var existCategory = DbContext.Categories.Find(item.CategoryCode);
                //existCategory.MaxPrice = DbContext.Flowers.Max(s => s.Price);
                //existCategory.MinPrice = DbContext.Flowers.Min(s => s.Price);
                //DbContext.Categories.AddOrUpdate(existCategory);
                //DbContext.SaveChanges();
                return true;
            }

            //ViewBag.ProductCategoryId = new SelectList(db.ProductCategories, "Id", "Name", product.ProductCategoryId);
            return false;
        }

        public bool Delete(Flower existFlower, ModelStateDictionary state)
        {
            if (state.IsValid)
            {
                existFlower.Status = FlowerStatus.Deleted;
                existFlower.DeletedAt = DateTime.Now;
                existFlower.DeletedBy = userService.GetCurrentUserName();
                DbContext.Flowers.AddOrUpdate(existFlower);
                DbContext.SaveChanges();

                return true;
            }

            return false;
        }

        public Flower Detail(string id)
        {
            return DbContext.Flowers.Find(id);
        }

        public void DisposeDb()
        {
            DbContext.Dispose();
        }

        public IEnumerable<Flower> GetList()
        {
            return DbContext.Flowers.Where(s => s.Status != FlowerStatus.Deleted).ToList();
        }

        public bool Update(Flower existItem, Flower item, ModelStateDictionary state)
        {
            throw new NotImplementedException();
        }

        public bool UpdateWithImage(Flower existItem, Flower item, ModelStateDictionary state, IEnumerable<HttpPostedFileBase> images)
        {
            ValidateCategory(item, state);
            if (state.IsValid)
            {
                existItem.Name = item.Name;
                existItem.Price = item.Price;
                existItem.CategoryCode = item.CategoryCode;
                //existItem.NumberOfLeture = item.NumberOfLeture;
                existItem.Description = item.Description;
                existItem.UpdatedAt = DateTime.Now;
                existItem.UpdatedBy = userService.GetCurrentUserName();
                //var list = existItem.ProductImages;
                DbContext.Flowers.AddOrUpdate(existItem);
                // add image to table ProductImages
                var imageList = mySQLImageService.SaveImage2List(item.Code, Constant.FlowerImage, images);
                DbContext.FlowerImages.AddRange(imageList);
                //
                var existCategory = DbContext.Categories.Find(existItem.CategoryCode);
                var flowers = DbContext.Flowers.Where(s => s.CategoryCode == existItem.CategoryCode);
                var maxFlowerPrice = (double?)flowers.Max(s => s.Price);
                var minFlowerPrice = (double?)flowers.Min(s => s.Price);
                if (maxFlowerPrice == null)
                    maxFlowerPrice = 0;
                if (minFlowerPrice == null)
                    minFlowerPrice = 0;
                if (existItem.Price > maxFlowerPrice)
                {
                    existCategory.MaxPrice = existItem.Price;
                    existCategory.MinPrice = minFlowerPrice;
                }
                else if (existItem.Price < minFlowerPrice)
                {
                    existCategory.MaxPrice = maxFlowerPrice;
                    existCategory.MinPrice = existItem.Price;
                }
                else
                {
                    existCategory.MaxPrice = maxFlowerPrice;
                    existCategory.MinPrice = minFlowerPrice;
                }
                DbContext.SaveChanges();

                return true;
            }

            return false;
        }

        public void Validate(Flower item, ModelStateDictionary state)
        {
            if (string.IsNullOrEmpty(item.Code))
            {
                state.AddModelError("Code", "Flower Code is required.");
            }
            var list = DbContext.Flowers.Where(s => s.Code == item.Code).ToList();
            if (list.Count != 0)
            {
                state.AddModelError("Code", "Flower Code already exist.");
            }
        }

        public void ValidateCategory(Flower item, ModelStateDictionary state)
        {
            if (string.IsNullOrEmpty(item.CategoryCode))
            {
                state.AddModelError("CategoryNameAndCode", "Category is required.");
            }
        }

        public bool ValidateStringCode(string code)
        {
            throw new NotImplementedException();
        }
    }
}