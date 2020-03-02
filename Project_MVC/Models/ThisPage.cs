using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project_MVC.Models
{
    public class ThisPage
    {
        public int CurrentPage { get; set; }
        public double TotalPage { get; set; }
        public string ProductCategoryCode { get; set; }
        public string CurrentType { get; set; }
        public string LectureId { get; set; }
        public string FunctionType { get; set; }
    }
}