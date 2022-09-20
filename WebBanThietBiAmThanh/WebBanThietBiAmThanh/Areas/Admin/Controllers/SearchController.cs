using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebBanThietBiAmThanh.Models;

namespace WebBanThietBiAmThanh.Areas.Admin.Controllers
{   
    [Area("Admin")]
    public class SearchController : Controller
    {
        private readonly db_maketContext _context;

        public SearchController(db_maketContext context)
        {
            _context = context;
        }

        [HttpPost]
        public IActionResult FindProduct(string keyword)
        {
            List<Product> ls = new List<Product>();
            if (string.IsNullOrEmpty(keyword) || keyword.Length < 1)
            {
                return PartialView("ListProductsSearchPartical", null);
            }
            ls = _context.Products
                .AsNoTracking()
                .Include(a => a.Cat)
                .Where(x => x.ProductName.Contains(keyword))
                .OrderByDescending(x => x.ProductName)
                .Take(10)
                .ToList();
            if (ls == null)
            {
                return PartialView("ListProductsSearchPartical", null);
            }
            else
            {
                return PartialView("ListProductsSearchPartical", ls);
            }
        }
    }
}
