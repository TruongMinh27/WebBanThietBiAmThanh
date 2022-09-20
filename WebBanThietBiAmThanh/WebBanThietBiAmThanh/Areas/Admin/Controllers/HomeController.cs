using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebBanThietBiAmThanh.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("admin", Name = "AdminIndex")]
    

    public class HomeController : Controller
    {
        public IActionResult Index()
        {
           
            return View();
        }
    }
}
