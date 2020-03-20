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

            var ratingCount = new RatingCount()
            {
                Code = flowerCode,
                NumberOfRating = 1
            };

            DbContext.RatingCounts.Add(ratingCount);
            DbContext.SaveChanges();
        }

        public string UpdateFlowerRating(decimal rating, string flowerCode, string type)
        {
            var existFlower = DbContext.Flowers.Find(flowerCode);
            var listRatingFlower = DbContext.RatingFlowers.Where(s => s.FlowerCode == flowerCode).ToList();
            var countRatingFlower = listRatingFlower.Count;
            if (countRatingFlower != 0 && listRatingFlower != null)
            {
                if (existFlower.Rating == null)
                {
                    existFlower.Rating = 0;
                }

                switch (type)
                {
                    case Constant.CreateRating:
                        existFlower.Rating = (existFlower.Rating + rating) / countRatingFlower;
                        break;
                    case Constant.UpdateRating:
                        existFlower.Rating = listRatingFlower.Select(s => s.Rating).Sum() / countRatingFlower;
                        break;
                    default:
                        break;
                }
            }

            DbContext.Flowers.AddOrUpdate(existFlower);
            DbContext.SaveChanges();

            return DbContext.Flowers.Find(flowerCode).Code;
        }

        public void UpdateRating(decimal rating, int? ratingFlowerId)
        {
            var existRatingFlower = DbContext.RatingFlowers.Find(ratingFlowerId);
            existRatingFlower.Rating = rating;
            DbContext.RatingFlowers.AddOrUpdate(existRatingFlower);

            var existRatingCount = DbContext.RatingCounts.Find(existRatingFlower.FlowerCode);
            if(existRatingCount.NumberOfRating == 0)
            {
                existRatingCount.NumberOfRating = DbContext.RatingFlowers.Where(s => s.FlowerCode == existRatingFlower.FlowerCode).Count();
            }
            else
            {
                existRatingCount.NumberOfRating++;
            }
            DbContext.RatingCounts.AddOrUpdate(existRatingCount);

            DbContext.SaveChanges();
        }
    }
}