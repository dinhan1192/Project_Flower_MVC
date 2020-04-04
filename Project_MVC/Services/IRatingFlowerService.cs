using Project_MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_MVC.Services
{
    interface IRatingFlowerService
    {
        void CreateRating(decimal rating, string flowerCode, string userId);
        decimal UpdateRating(decimal rating, int? ratingFlowerId);
        //void UpdateFlowerRating(string flowerCode);
        string UpdateFlowerRating(decimal rating, string flowerCode, string type, decimal oldRating);
        //CustomerLectureInteract DetailByLectureIdAndUserId(int? lectureId);
        //List<CustomerLectureInteract> GetListByLectureId(int? lectureId);
    }
}
