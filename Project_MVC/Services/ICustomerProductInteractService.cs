using Project_MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_MVC.Services
{
    interface ICustomerProductInteractService
    {
        void CreateRating(decimal rating, int? lectureId, string userId);
        void UpdateRating(decimal rating, int? customerLectureInteractId);
        void UpdateProductRating(string productCode);
        string UpdateLectureRating(decimal rating, int? lectureId, string type);
        //CustomerLectureInteract DetailByLectureIdAndUserId(int? lectureId);
        //List<CustomerLectureInteract> GetListByLectureId(int? lectureId);
    }
}
