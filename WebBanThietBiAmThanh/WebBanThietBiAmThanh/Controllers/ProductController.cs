using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PagedList.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebBanThietBiAmThanh.Models;
using WebBanThietBiAmThanh.ModelViews;

namespace WebBanThietBiAmThanh.Controllers
{
    public class ProductController : Controller
    {
        private readonly db_maketContext _context;

        public ProductController(db_maketContext context)
        {
            _context = context;
        }
        [Route("/shop.html", Name = "Shop")]
        public IActionResult Index(int? page = 1)
        {
            try
            {
                var pageNumber = page == null || page <= 0 ? 1 : page.Value;
                var pageSize = 8;
                
                var lsProducts = _context.Products
                    .AsNoTracking()
                    .OrderByDescending(x => x.DateCreated);

                PagedList<Product> models = new PagedList<Product>(lsProducts, pageNumber, pageSize);
                ViewBag.CurrentPage = page;

                return View(models);
            }
            catch
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [Route("/{Alias}", Name = "ListProduct")]
        public IActionResult List(string Alias, int page = 1)
        {
            try
            {
                var pageSize = 8;
                var danhmuc = _context.Categories.AsNoTracking().SingleOrDefault(x =>x.Alias == Alias);
                var lsProducts = _context.Products
                    .Where(x => x.CatId == danhmuc.CatId)
                    .AsNoTracking()
                    .OrderByDescending(x => x.DateCreated);

                PagedList<Product> models = new PagedList<Product>(lsProducts, page, pageSize);
                ViewBag.CurrentPage = page;
                ViewBag.CurrentCat = danhmuc;

                return View(models);
            }
            catch
            {
                return RedirectToAction("Index", "Home");
            }
           

        }
        [Route("/{Alias}-{id}.html", Name = "ProductDetails")]
        public IActionResult Details(int id)
        {
            try
            {
                var product = _context.Products.Include(x => x.Cat).FirstOrDefault(x => x.ProductId == id);
                if (product == null)
                {
                    return RedirectToAction("Index");
                }
                var lsSanPhamlienquan = _context.Products
                .AsNoTracking()
                .Where(x => x.CatId == product.CatId && x.ProductId != id && x.Active == true)
                .Take(4)
                .OrderByDescending(x => x.DateCreated).ToList();
                ViewBag.SanPhamlienquan = lsSanPhamlienquan;

                return View(product);
                
            }
            catch
            {
                return RedirectToAction("Index", "Home");
            }
            
            


        }
    }
}
