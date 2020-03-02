//using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
//using Newtonsoft.Json.Linq;
//using Newtonsoft.Json.Schema;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project_MVC.Models
{
    public class Product
    {
        [Key]
        [DisplayName("Product Code")]
        public string Code { get; set; }
        [DisplayName("Product Name")]
        [Required]
        [RegularExpression(@"^[0-9a-zA-Z\s+ÀÁÂÃÈÉÊÌÍÒÓÔÕÙÚĂĐĨŨƠàáâãèéêìíòóôõùúăđĩũơƯĂẠẢẤẦẨẪẬẮẰẲẴẶẸẺẼỀỀỂưăạảấầẩẫậắằẳẵặẹẻẽềếểỄỆỈỊỌỎỐỒỔỖỘỚỜỞỠỢỤỦỨỪễệỉịọỏốồổỗộớờởỡợụủứừỬỮỰỲỴÝỶỸửữựỳỵỷỹ]+$", ErrorMessage = "Invalid Product Name")]
        public string Name { get; set; }
        //[Required]
        //[DisplayName("Number of Lectures")]
        //[Range(1, Int32.MaxValue, ErrorMessage = "Number of Lectures can not be smaller than 1")]
        //public int NumberOfLeture { get; set; }
        [Required]
        public double Price { get; set; }
        public decimal? Rating { get; set; }
        public string Description { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public string DeletedBy { get; set; }
        public ProductStatus Status { get; set; }
        [ForeignKey("ProductCategory")]
        public string ProductCategoryCode { get; set; }
        public virtual ProductCategory ProductCategory { get; set; }
        [DisplayName("Product Category")]
        [NotMapped]
        [RegularExpression(@"^[0-9A-Z]+\s-\s[0-9a-zA-Z\s+ÀÁÂÃÈÉÊÌÍÒÓÔÕÙÚĂĐĨŨƠàáâãèéêìíòóôõùúăđĩũơƯĂẠẢẤẦẨẪẬẮẰẲẴẶẸẺẼỀỀỂưăạảấầẩẫậắằẳẵặẹẻẽềếểỄỆỈỊỌỎỐỒỔỖỘỚỜỞỠỢỤỦỨỪễệỉịọỏốồổỗộớờởỡợụủứừỬỮỰỲỴÝỶỸửữựỳỵỷỹ]+$", ErrorMessage = "Invalid Product Category")]
        public string ProductCategoryNameAndCode { get; set; }
        //[NotMapped]
        //[DisplayName("Product Image")]
        //public HttpPostedFileBase ProductImageFile { get; set; }
        #region Images members
        public virtual ICollection<ProductImage> ProductImages { get; set; }
        #endregion
        #region Lectures members
        public virtual ICollection<Lecture> Lectures { get; set; }
        #endregion
        #region Owner of Course members

        [ForeignKey("OwnerOfCourse")]
        public string OwnerOfCourseCode { get; set; }
        public virtual OwnerOfCourse OwnerOfCourse { get; set; }
        [DisplayName("Owner Of Course")]
        [NotMapped]
        [RegularExpression(@"^[0-9A-Z]+\s-\s[0-9a-zA-Z\s+ÀÁÂÃÈÉÊÌÍÒÓÔÕÙÚĂĐĨŨƠàáâãèéêìíòóôõùúăđĩũơƯĂẠẢẤẦẨẪẬẮẰẲẴẶẸẺẼỀỀỂưăạảấầẩẫậắằẳẵặẹẻẽềếểỄỆỈỊỌỎỐỒỔỖỘỚỜỞỠỢỤỦỨỪễệỉịọỏốồổỗộớờởỡợụủứừỬỮỰỲỴÝỶỸửữựỳỵỷỹ]+$", ErrorMessage = "Invalid Owner of Course")]
        public string OwnerOfCourseNameAndCode { get; set; }

        #endregion

        #region User Products Members

        public virtual ICollection<UserProduct> UserProducts { get; set; }

        #endregion

        public enum ProductStatus
        {
            NotDeleted = 0, Deleted = -1
        }

        internal bool IsDeleted()
        {
            return this.Status == ProductStatus.Deleted;
        }
    }
}