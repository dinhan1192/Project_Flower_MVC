using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Project_MVC.Models
{
    public class AppRole : IdentityRole
    {
        public AppRole() : base()
        {
        }
        public AppRole(string name) : base(name) { }

        [NotMapped]
        public bool isChoosen { get; set; }
        public string Description { get; set; }
        [DisplayName("Created At")]
        public DateTime? CreatedAt { get; set; }
        [DisplayName("Updated At")]
        public DateTime? UpdatedAt { get; set; }
        [DisplayName("Deleted At")]
        public DateTime? DeletedAt { get; set; }
    }
}