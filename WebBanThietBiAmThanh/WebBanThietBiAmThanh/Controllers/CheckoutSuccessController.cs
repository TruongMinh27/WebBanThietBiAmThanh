using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebBanThietBiAmThanh.Models;

namespace WebBanThietBiAmThanh.Controllers
{
    public class CheckoutSuccessController : Controller
    {
        private readonly db_maketContext _context;

        public CheckoutSuccessController(db_maketContext context)
        {
            _context = context;
        }
        [Route("checkout-success.html")]
        public IActionResult Index()
        {
            HttpContext.Session.Remove("GioHang");
            return View();
        }
    }
}
