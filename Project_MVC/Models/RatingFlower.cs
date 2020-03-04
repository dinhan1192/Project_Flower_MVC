using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Project_MVC.Models
{
    public class RatingFlower
    {
        [Key]
        public int? Id { get; set; }
        [ForeignKey("Flower")]
        public string FlowerCode { get; set; }
        [ForeignKey("AppUser")]
        public string UserId { get; set; }
        public virtual AppUser AppUser { get; set; }
        public virtual Flower Flower { get; set; }

        #region Rating Property

        public decimal Rating { get; set; }

        #endregion
    }
}