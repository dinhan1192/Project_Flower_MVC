using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Project_MVC.Models
{
    //[DbConfigurationType(typeof(MySql.Data.Entity.MySqlEFConfiguration))]
    public class MyDbContext : IdentityDbContext<AppUser>
    {
        public MyDbContext() : base("name=SQLContext")
        {
            //this.Configuration.ProxyCreationEnabled = false;
            //this.Configuration.LazyLoadingEnabled = false;
        }

        public static MyDbContext Create()
        {
            return new MyDbContext();
        }

        public DbSet<AppRole> IdentityRoles { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Flower> Flowers { get; set; }
        public DbSet<FlowerImage> FlowerImages { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<RatingFlower> RatingFlowers { get; set; }
        public DbSet<IdCount> IdCounts { get; set; }
        public DbSet<RatingCount> RatingCounts { get; set; }
    }
}