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
        public string LevelOneCategoryCode { get; set; }
        public string CurrentType { get; set; }
        public string LectureId { get; set; }
        public string FunctionType { get; set; }

        #region AdminPage

        public string SearchString { get; set; }
        public string PaymentType { get; set; }
        public string Status { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }

        #endregion

        #region CustomerPage

        public string Amount { get; set; }
        public string SortFlower { get; set; }

        #endregion
    }
}