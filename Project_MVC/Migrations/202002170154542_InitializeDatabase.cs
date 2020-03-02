namespace Project_MVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitializeDatabase : DbMigration
    {
        public override void Up()
        {
            //CreateTable(
            //    "dbo.CustomerLectureInteracts",
            //    c => new
            //        {
            //            Id = c.Int(nullable: false, identity: true),
            //            LectureId = c.Int(),
            //            UserId = c.String(maxLength: 128),
            //            Rating = c.Decimal(nullable: false, precision: 18, scale: 2),
            //        })
            //    .PrimaryKey(t => t.Id)
            //    .ForeignKey("dbo.AspNetUsers", t => t.UserId)
            //    .ForeignKey("dbo.Lectures", t => t.LectureId)
            //    .Index(t => t.LectureId)
            //    .Index(t => t.UserId);
            
            //CreateTable(
            //    "dbo.AspNetUsers",
            //    c => new
            //        {
            //            Id = c.String(nullable: false, maxLength: 128),
            //            FirstName = c.String(),
            //            LastName = c.String(),
            //            Gender = c.Int(nullable: false),
            //            BirthDay = c.DateTime(),
            //            CreatedAt = c.DateTime(),
            //            UpdatedAt = c.DateTime(),
            //            DeletedAt = c.DateTime(),
            //            Email = c.String(maxLength: 256),
            //            EmailConfirmed = c.Boolean(nullable: false),
            //            PasswordHash = c.String(),
            //            SecurityStamp = c.String(),
            //            PhoneNumber = c.String(),
            //            PhoneNumberConfirmed = c.Boolean(nullable: false),
            //            TwoFactorEnabled = c.Boolean(nullable: false),
            //            LockoutEndDateUtc = c.DateTime(),
            //            LockoutEnabled = c.Boolean(nullable: false),
            //            AccessFailedCount = c.Int(nullable: false),
            //            UserName = c.String(nullable: false, maxLength: 256),
            //        })
            //    .PrimaryKey(t => t.Id)
            //    .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            //CreateTable(
            //    "dbo.AspNetUserClaims",
            //    c => new
            //        {
            //            Id = c.Int(nullable: false, identity: true),
            //            UserId = c.String(nullable: false, maxLength: 128),
            //            ClaimType = c.String(),
            //            ClaimValue = c.String(),
            //        })
            //    .PrimaryKey(t => t.Id)
            //    .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
            //    .Index(t => t.UserId);
            
            //CreateTable(
            //    "dbo.AspNetUserLogins",
            //    c => new
            //        {
            //            LoginProvider = c.String(nullable: false, maxLength: 128),
            //            ProviderKey = c.String(nullable: false, maxLength: 128),
            //            UserId = c.String(nullable: false, maxLength: 128),
            //        })
            //    .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
            //    .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
            //    .Index(t => t.UserId);
            
            //CreateTable(
            //    "dbo.AspNetUserRoles",
            //    c => new
            //        {
            //            UserId = c.String(nullable: false, maxLength: 128),
            //            RoleId = c.String(nullable: false, maxLength: 128),
            //        })
            //    .PrimaryKey(t => new { t.UserId, t.RoleId })
            //    .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
            //    .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
            //    .Index(t => t.UserId)
            //    .Index(t => t.RoleId);
            
            //CreateTable(
            //    "dbo.UserProducts",
            //    c => new
            //        {
            //            Id = c.Int(nullable: false, identity: true),
            //            ProductCode = c.String(maxLength: 128),
            //            UserId = c.String(maxLength: 128),
            //        })
            //    .PrimaryKey(t => t.Id)
            //    .ForeignKey("dbo.AspNetUsers", t => t.UserId)
            //    .ForeignKey("dbo.Products", t => t.ProductCode)
            //    .Index(t => t.ProductCode)
            //    .Index(t => t.UserId);
            
            //CreateTable(
            //    "dbo.Products",
            //    c => new
            //        {
            //            Code = c.String(nullable: false, maxLength: 128),
            //            Name = c.String(nullable: false),
            //            Price = c.Double(nullable: false),
            //            Rating = c.Int(nullable: false),
            //            Description = c.String(),
            //            CreatedAt = c.DateTime(),
            //            UpdatedAt = c.DateTime(),
            //            DeletedAt = c.DateTime(),
            //            CreatedBy = c.String(),
            //            UpdatedBy = c.String(),
            //            DeletedBy = c.String(),
            //            Status = c.Int(nullable: false),
            //            ProductCategoryCode = c.String(maxLength: 128),
            //            OwnerOfCourseCode = c.String(maxLength: 128),
            //        })
            //    .PrimaryKey(t => t.Code)
            //    .ForeignKey("dbo.OwnerOfCourses", t => t.OwnerOfCourseCode)
            //    .ForeignKey("dbo.ProductCategories", t => t.ProductCategoryCode)
            //    .Index(t => t.ProductCategoryCode)
            //    .Index(t => t.OwnerOfCourseCode);
            
            //CreateTable(
            //    "dbo.Lectures",
            //    c => new
            //        {
            //            Id = c.Int(nullable: false, identity: true),
            //            Name = c.String(nullable: false),
            //            Description = c.String(),
            //            DisplayOrder = c.Int(nullable: false),
            //            CreatedAt = c.DateTime(),
            //            UpdatedAt = c.DateTime(),
            //            DeletedAt = c.DateTime(),
            //            CreatedBy = c.String(),
            //            UpdatedBy = c.String(),
            //            DeletedBy = c.String(),
            //            Status = c.Int(nullable: false),
            //            ProductCode = c.String(maxLength: 128),
            //        })
            //    .PrimaryKey(t => t.Id)
            //    .ForeignKey("dbo.Products", t => t.ProductCode)
            //    .Index(t => t.ProductCode);
            
            //CreateTable(
            //    "dbo.LectureVideos",
            //    c => new
            //        {
            //            Id = c.Int(nullable: false, identity: true),
            //            Name = c.String(),
            //            VideoData = c.Binary(),
            //            ContentType = c.String(),
            //            LectureId = c.Int(),
            //            DisplayOrder = c.Int(nullable: false),
            //            CreatedAt = c.DateTime(),
            //            CreatedBy = c.String(),
            //            Rating = c.Int(nullable: false),
            //            TotalRaters = c.Int(nullable: false),
            //            AverageRating = c.Double(nullable: false),
            //        })
            //    .PrimaryKey(t => t.Id)
            //    .ForeignKey("dbo.Lectures", t => t.LectureId)
            //    .Index(t => t.LectureId);
            
            //CreateTable(
            //    "dbo.OwnerOfCourses",
            //    c => new
            //        {
            //            Code = c.String(nullable: false, maxLength: 128),
            //            Name = c.String(nullable: false),
            //            Occupation = c.String(nullable: false),
            //            Description = c.String(),
            //            ImageData = c.Binary(),
            //            CreatedAt = c.DateTime(),
            //            UpdatedAt = c.DateTime(),
            //            DeletedAt = c.DateTime(),
            //            CreatedBy = c.String(),
            //            UpdatedBy = c.String(),
            //            DeletedBy = c.String(),
            //            ProductCategoryCode = c.String(maxLength: 128),
            //            Status = c.Int(nullable: false),
            //        })
            //    .PrimaryKey(t => t.Code)
            //    .ForeignKey("dbo.ProductCategories", t => t.ProductCategoryCode)
            //    .Index(t => t.ProductCategoryCode);
            
            //CreateTable(
            //    "dbo.ProductCategories",
            //    c => new
            //        {
            //            Code = c.String(nullable: false, maxLength: 128),
            //            Name = c.String(nullable: false),
            //            Description = c.String(),
            //            CreatedAt = c.DateTime(),
            //            UpdatedAt = c.DateTime(),
            //            DeletedAt = c.DateTime(),
            //            CreatedBy = c.String(),
            //            UpdatedBy = c.String(),
            //            DeletedBy = c.String(),
            //            Status = c.Int(nullable: false),
            //            LevelOneProductCategoryCode = c.String(maxLength: 128),
            //        })
            //    .PrimaryKey(t => t.Code)
            //    .ForeignKey("dbo.LevelOneProductCategories", t => t.LevelOneProductCategoryCode)
            //    .Index(t => t.LevelOneProductCategoryCode);
            
            //CreateTable(
            //    "dbo.LevelOneProductCategories",
            //    c => new
            //        {
            //            Code = c.String(nullable: false, maxLength: 128),
            //            Name = c.String(nullable: false),
            //            Description = c.String(),
            //            CreatedAt = c.DateTime(),
            //            UpdatedAt = c.DateTime(),
            //            DeletedAt = c.DateTime(),
            //            CreatedBy = c.String(),
            //            UpdatedBy = c.String(),
            //            DeletedBy = c.String(),
            //            Status = c.Int(nullable: false),
            //        })
            //    .PrimaryKey(t => t.Code);
            
            //CreateTable(
            //    "dbo.ProductImages",
            //    c => new
            //        {
            //            Id = c.Int(nullable: false, identity: true),
            //            ProductCode = c.String(maxLength: 128),
            //            ImageData = c.Binary(),
            //            CreatedAt = c.DateTime(),
            //            CreatedBy = c.String(),
            //        })
            //    .PrimaryKey(t => t.Id)
            //    .ForeignKey("dbo.Products", t => t.ProductCode)
            //    .Index(t => t.ProductCode);
            
            //CreateTable(
            //    "dbo.AspNetRoles",
            //    c => new
            //        {
            //            Id = c.String(nullable: false, maxLength: 128),
            //            Name = c.String(nullable: false, maxLength: 256),
            //            Description = c.String(),
            //            CreatedAt = c.DateTime(),
            //            UpdatedAt = c.DateTime(),
            //            DeletedAt = c.DateTime(),
            //            Discriminator = c.String(nullable: false, maxLength: 128),
            //        })
            //    .PrimaryKey(t => t.Id)
            //    .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            //CreateTable(
            //    "dbo.LevelOneMenus",
            //    c => new
            //        {
            //            Code = c.String(nullable: false, maxLength: 128),
            //            Name = c.String(nullable: false),
            //            ActionName = c.String(),
            //            ControllerName = c.String(),
            //            Description = c.String(),
            //            CreatedAt = c.DateTime(),
            //            UpdatedAt = c.DateTime(),
            //            DeletedAt = c.DateTime(),
            //            CreatedBy = c.String(),
            //            UpdatedBy = c.String(),
            //            DeletedBy = c.String(),
            //            Status = c.Int(nullable: false),
            //        })
            //    .PrimaryKey(t => t.Code);
            
            //CreateTable(
            //    "dbo.LevelTwoMenus",
            //    c => new
            //        {
            //            Code = c.String(nullable: false, maxLength: 128),
            //            Name = c.String(nullable: false),
            //            ActionName = c.String(),
            //            ControllerName = c.String(),
            //            Description = c.String(),
            //            CreatedAt = c.DateTime(),
            //            UpdatedAt = c.DateTime(),
            //            DeletedAt = c.DateTime(),
            //            CreatedBy = c.String(),
            //            UpdatedBy = c.String(),
            //            DeletedBy = c.String(),
            //            Status = c.Int(nullable: false),
            //            LevelOneMenuCode = c.String(maxLength: 128),
            //        })
            //    .PrimaryKey(t => t.Code)
            //    .ForeignKey("dbo.LevelOneMenus", t => t.LevelOneMenuCode)
            //    .Index(t => t.LevelOneMenuCode);
            
            //CreateTable(
            //    "dbo.OrderDetails",
            //    c => new
            //        {
            //            ProductCode = c.String(nullable: false, maxLength: 128),
            //            OrderId = c.Int(nullable: false),
            //            Quantity = c.Int(nullable: false),
            //            UnitPrice = c.Double(nullable: false),
            //        })
            //    .PrimaryKey(t => new { t.ProductCode, t.OrderId })
            //    .ForeignKey("dbo.Orders", t => t.OrderId, cascadeDelete: true)
            //    .ForeignKey("dbo.Products", t => t.ProductCode, cascadeDelete: true)
            //    .Index(t => t.ProductCode)
            //    .Index(t => t.OrderId);
            
            //CreateTable(
            //    "dbo.Orders",
            //    c => new
            //        {
            //            Id = c.Int(nullable: false, identity: true),
            //            MemberId = c.Int(nullable: false),
            //            PaymentTypeId = c.Int(nullable: false),
            //            ShipName = c.String(),
            //            ShipAddress = c.String(),
            //            ShipPhone = c.String(),
            //            TotalPrice = c.Double(nullable: false),
            //            CreatedAt = c.DateTime(),
            //            UpdatedAt = c.DateTime(),
            //            DeletedAt = c.DateTime(),
            //            CreatedBy = c.String(),
            //            UpdatedBy = c.String(),
            //            DeletedBy = c.String(),
            //            Status = c.Int(nullable: false),
            //        })
            //    .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.OrderDetails", "ProductCode", "dbo.Products");
            DropForeignKey("dbo.OrderDetails", "OrderId", "dbo.Orders");
            DropForeignKey("dbo.LevelTwoMenus", "LevelOneMenuCode", "dbo.LevelOneMenus");
            DropForeignKey("dbo.CustomerLectureInteracts", "LectureId", "dbo.Lectures");
            DropForeignKey("dbo.CustomerLectureInteracts", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.UserProducts", "ProductCode", "dbo.Products");
            DropForeignKey("dbo.ProductImages", "ProductCode", "dbo.Products");
            DropForeignKey("dbo.Products", "ProductCategoryCode", "dbo.ProductCategories");
            DropForeignKey("dbo.Products", "OwnerOfCourseCode", "dbo.OwnerOfCourses");
            DropForeignKey("dbo.OwnerOfCourses", "ProductCategoryCode", "dbo.ProductCategories");
            DropForeignKey("dbo.ProductCategories", "LevelOneProductCategoryCode", "dbo.LevelOneProductCategories");
            DropForeignKey("dbo.Lectures", "ProductCode", "dbo.Products");
            DropForeignKey("dbo.LectureVideos", "LectureId", "dbo.Lectures");
            DropForeignKey("dbo.UserProducts", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.OrderDetails", new[] { "OrderId" });
            DropIndex("dbo.OrderDetails", new[] { "ProductCode" });
            DropIndex("dbo.LevelTwoMenus", new[] { "LevelOneMenuCode" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.ProductImages", new[] { "ProductCode" });
            DropIndex("dbo.ProductCategories", new[] { "LevelOneProductCategoryCode" });
            DropIndex("dbo.OwnerOfCourses", new[] { "ProductCategoryCode" });
            DropIndex("dbo.LectureVideos", new[] { "LectureId" });
            DropIndex("dbo.Lectures", new[] { "ProductCode" });
            DropIndex("dbo.Products", new[] { "OwnerOfCourseCode" });
            DropIndex("dbo.Products", new[] { "ProductCategoryCode" });
            DropIndex("dbo.UserProducts", new[] { "UserId" });
            DropIndex("dbo.UserProducts", new[] { "ProductCode" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.CustomerLectureInteracts", new[] { "UserId" });
            DropIndex("dbo.CustomerLectureInteracts", new[] { "LectureId" });
            DropTable("dbo.Orders");
            DropTable("dbo.OrderDetails");
            DropTable("dbo.LevelTwoMenus");
            DropTable("dbo.LevelOneMenus");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.ProductImages");
            DropTable("dbo.LevelOneProductCategories");
            DropTable("dbo.ProductCategories");
            DropTable("dbo.OwnerOfCourses");
            DropTable("dbo.LectureVideos");
            DropTable("dbo.Lectures");
            DropTable("dbo.Products");
            DropTable("dbo.UserProducts");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.CustomerLectureInteracts");
        }
    }
}
