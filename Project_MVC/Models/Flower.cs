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
    public class Flower
    {
        [Key]
        [DisplayName("Flower Code")]
        public string Code { get; set; }
        [DisplayName("Flower Name")]
        [Required]
        [RegularExpression(@"^[0-9a-zA-Z\s+ÀÁÂÃÈÉÊÌÍÒÓÔÕÙÚĂĐĨŨƠàáâãèéêìíòóôõùúăđĩũơƯĂẠẢẤẦẨẪẬẮẰẲẴẶẸẺẼỀỀỂưăạảấầẩẫậắằẳẵặẹẻẽềếểỄỆỈỊỌỎỐỒỔỖỘỚỜỞỠỢỤỦỨỪễệỉịọỏốồổỗộớờởỡợụủứừỬỮỰỲỴÝỶỸửữựỳỵỷỹ]+$", ErrorMessage = "Invalid Product Name")]
        public string Name { get; set; }
        //[Required]
        //[DisplayName("Number of Lectures")]
        //[Range(1, Int32.MaxValue, ErrorMessage = "Number of Lectures can not be smaller than 1")]
        //public int NumberOfLeture { get; set; }
        [Required]
        [DisplayName("Price (VNĐ)")]
        public double Price { get; set; }
        [DisplayName("Discount (%)")]
        public double Discount { get; set; }
        public decimal? Rating { get; set; }
        public string Description { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public string DeletedBy { get; set; }
        public FlowerStatus Status { get; set; }
        [ForeignKey("Category")]
        public string CategoryCode { get; set; }
        public virtual Category Category { get; set; }
        [DisplayName("Category")]
        [NotMapped]
        [RegularExpression(@"^[0-9A-Z]+\s-\s[0-9a-zA-Z\s+ÀÁÂÃÈÉÊÌÍÒÓÔÕÙÚĂĐĨŨƠàáâãèéêìíòóôõùúăđĩũơƯĂẠẢẤẦẨẪẬẮẰẲẴẶẸẺẼỀỀỂưăạảấầẩẫậắằẳẵặẹẻẽềếểỄỆỈỊỌỎỐỒỔỖỘỚỜỞỠỢỤỦỨỪễệỉịọỏốồổỗộớờởỡợụủứừỬỮỰỲỴÝỶỸửữựỳỵỷỹ]+$", ErrorMessage = "Invalid Product Category")]
        public string CategoryNameAndCode { get; set; }
        //[NotMapped]
        //[DisplayName("Product Image")]
        //public HttpPostedFileBase ProductImageFile { get; set; }
        #region Images members
        public virtual ICollection<FlowerImage> FlowerImages { get; set; }
        #endregion
       
        #region Rating Members

        public virtual ICollection<RatingFlower> RatingFlowers { get; set; }

        #endregion

        public enum FlowerStatus
        {
            NotDeleted = 0, Deleted = -1
        }

        internal bool IsDeleted()
        {
            return this.Status == FlowerStatus.Deleted;
        }
    }
}