using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PagedList.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebBanThietBiAmThanh.Models;

namespace WebBanThietBiAmThanh.Controllers
{
    public class BlogController : Controller
    {
        private readonly db_maketContext _context;

        public BlogController(db_maketContext context)
        {
            _context = context;
        }
        // GET: Admin/AdminProducts
        [Route("tin-tuc.html", Name = "Blog")]
        public IActionResult Index(int page = 1, int PostID = 0)
        {
            var pageNumber = page;
            var pageSize = 5;

            List<TinTuc> lsTintucs = new List<TinTuc>();
            if (PostID != 0)
            {
                lsTintucs = _context.TinTucs
                .AsNoTracking()
                .Where(x => x.PostId == PostID)
                .OrderByDescending(x => x.PostId).ToList();
            }
            else
            {
                lsTintucs = _context.TinTucs
                    .AsNoTracking()
                    .OrderByDescending(x => x.PostId).ToList();
            }


            var IsBaivietlienquan = _context.TinTucs
                .AsNoTracking().Where(x => x.Published == true)
                .Take(5)
                .OrderByDescending(x => x.CreatedDate).ToList();
            ViewBag.Baivietlienquan = IsBaivietlienquan;

            var IsDanhMucSanPham = _context.Categories
                .AsNoTracking().Where(x => x.Published == true)
                .Take(3)
                .OrderByDescending(x => x.CatName).ToList();
            ViewBag.DanhMucSanPham = IsDanhMucSanPham;

            PagedList<TinTuc> models = new PagedList<TinTuc>(lsTintucs.AsQueryable(), pageNumber, pageSize);
            ViewBag.CurrentPagePostID= PostID;
            ViewBag.CurrentPage = pageNumber;
            ViewData["DanhMuc"] = new SelectList(_context.TinTucs, "PostId", "Title", PostID);
            return View(models);
        }
        [Route("tin-tuc/{Alias}-{id}.html",Name = "TinDetails")]
        public IActionResult Details(int id)
        {
            var tintuc = _context.TinTucs.AsNoTracking().SingleOrDefault(x => x.PostId == id);
            if (tintuc == null)
            {
                return RedirectToAction("Index");
            }
            var IsBaivietlienquan = _context.TinTucs
                .AsNoTracking().Where(x => x.Published == true && x.PostId != id)
                .Take(5)
                .OrderByDescending(x => x.CreatedDate).ToList();
            ViewBag.Baivietlienquan = IsBaivietlienquan;

            var IsDanhMucSanPham = _context.Categories
                .AsNoTracking().Where(x => x.Published == true && x.CatId != id)
                .Take(3)
                .OrderByDescending(x => x.CatName).ToList();
            ViewBag.DanhMucSanPham = IsDanhMucSanPham;
            return View(tintuc);
        }
    }
}
