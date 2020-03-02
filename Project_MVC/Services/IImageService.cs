using Project_MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Project_MVC.Services
{
    interface IImageService
    {
        List<ProductImage> SaveImage2List(string code, int? type, IEnumerable<HttpPostedFileBase> images);
        List<LectureVideo> SaveVideo2List(int? id, IEnumerable<HttpPostedFileBase> videos, ModelStateDictionary state);
        LectureVideo Detail(int? fileId);
        bool Rating(decimal rating, int? lectureVideoId);
        bool Delete(LectureVideo existItem, ModelStateDictionary state);
        bool ValidateVideo(string videoName, ModelStateDictionary state);
        void ValidateVideoDisplayOrder(int displayOrder, int parentId, ModelStateDictionary state);
    }
}
