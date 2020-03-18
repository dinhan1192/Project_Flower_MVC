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
        List<FlowerImage> SaveImage2List(string code, int? type, string strImageUrl);
        //List<LectureVideo> SaveVideo2List(int? id, IEnumerable<HttpPostedFileBase> videos, ModelStateDictionary state);
        //LectureVideo Detail(int? fileId);
        bool Rating(decimal rating, string code);
        FlowerImage DetailImage(int? id);
        void DeleteImage(FlowerImage flowerImage);
        //bool Delete(LectureVideo existItem, ModelStateDictionary state);
        bool ValidateVideo(string videoName, ModelStateDictionary state);
        void ValidateVideoDisplayOrder(int displayOrder, int parentId, ModelStateDictionary state);
        List<FlowerImage> GetList();
    }
}
