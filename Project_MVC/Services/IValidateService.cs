using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_MVC.Services
{
    interface IValidateService
    {
        bool ValidateUserProducts(string code, string id);
    }
}
