using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Project_MVC.Models
{
    public class FlowerImage
    {
        public int? Id { get; set; }
        [ForeignKey("Flower")]
        public string FlowerCode { get; set; }
        //[ForeignKey("OwnerOfCourse")]
        //public string OwnerOfCourseCode { get; set; }
        [DisplayName("Image")]
        public string ImageUrl { get; set; }
        public virtual Flower Flower { get; set; }
        //public virtual OwnerOfCourse OwnerOfCourse { get; set; }
        [DisplayName("Created At")]
        public DateTime? CreatedAt { get; set; }
        [DisplayName("Created By")]
        public string CreatedBy { get; set; }
    }
}