using Microsoft.AspNet.Identity.Owin;
using Project_MVC.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;

namespace Project_MVC.Services
{
    public class RatingFlowerService : IRatingFlowerService
    {
        private MyDbContext _db;
        public MyDbContext DbContext
        {
            get { return _db ?? HttpContext.Current.GetOwinContext().Get<MyDbContext>(); }
            set { _db = value; }
        }

        public void CreateRating(decimal rating, string flowerCode, string userId)
        {
            var item = new RatingFlower()
            {
                FlowerCode = flowerCode,
                UserId = userId,
                Rating = rating
            };

            DbContext.RatingFlowers.Add(item);

            var existRatingCount = DbContext.RatingCounts.Find(item.FlowerCode);
            if (existRatingCount != null)
            {
                if (existRatingCount.NumberOfRating == 0)
                {
                    existRatingCount.NumberOfRating = DbContext.RatingFlowers.Where(s => s.FlowerCode == item.FlowerCode).Count();
                }
                else
                {
                    existRatingCount.NumberOfRating++;
                }

                DbContext.RatingCounts.AddOrUpdate(existRatingCount);
            }
            else
            {
                var ratingCount = new RatingCount()
                {
                    Code = item.FlowerCode,
                    NumberOfRating = DbContext.RatingFlowers.Where(s => s.FlowerCode == item.FlowerCode).Count() + 1
                };

                DbContext.RatingCounts.Add(ratingCount);
            }

            //var ratingCount = new RatingCount()
            //{
            //    Code = flowerCode,
            //    NumberOfRating = 1
            //};

            //DbContext.RatingCounts.Add(ratingCount);
            DbContext.SaveChanges();
        }

        public string UpdateFlowerRating(decimal rating, string flowerCode, string type, decimal oldRating)
        {
            var existFlower = DbContext.Flowers.Find(flowerCode);
            var listRatingFlower = DbContext.RatingFlowers.Where(s => s.FlowerCode == flowerCode).ToList();
            var countRatingFlower = DbContext.RatingCounts.Where(s => s.Code == flowerCode).FirstOrDefault();
            if (countRatingFlower != null && listRatingFlower != null)
            {
                if (existFlower.Rating == null)
                {
                    existFlower.Rating = 0;
                }

                switch (type)
                {
                    case Constant.CreateRating:
                        existFlower.Rating = (existFlower.Rating + rating) / countRatingFlower.NumberOfRating;
                        break;
                    case Constant.UpdateRating:
                        existFlower.Rating = (existFlower.Rating * countRatingFlower.NumberOfRating - oldRating + rating) / countRatingFlower.NumberOfRating;
                        break;
                    default:
                        break;
                }
            }

            DbContext.Flowers.AddOrUpdate(existFlower);
            DbContext.SaveChanges();

            return DbContext.Flowers.Find(flowerCode).Code;
        }

        public decimal UpdateRating(decimal rating, int? ratingFlowerId)
        {
            var existRatingFlower = DbContext.RatingFlowers.Find(ratingFlowerId);
            var oldRating = existRatingFlower.Rating;
            existRatingFlower.Rating = rating;
            DbContext.RatingFlowers.AddOrUpdate(existRatingFlower);

            var existRatingCount = DbContext.RatingCounts.Find(existRatingFlower.FlowerCode);
            if(existRatingCount == null)
            {
                var ratingCount = new RatingCount()
                {
                    Code = existRatingFlower.FlowerCode,
                    NumberOfRating = DbContext.RatingFlowers.Where(s => s.FlowerCode == existRatingFlower.FlowerCode).Count()
                };

                DbContext.RatingCounts.Add(ratingCount);
            }
            
            DbContext.SaveChanges();

            return oldRating;
        }
    }
}