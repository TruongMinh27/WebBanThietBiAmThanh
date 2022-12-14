using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WebBanThietBiAmThanh.Extension;
using WebBanThietBiAmThanh.Helpers;
using WebBanThietBiAmThanh.Models;
using WebBanThietBiAmThanh.ModelViews;

namespace WebBanThietBiAmThanh.Controllers
{
    [Authorize]
    public class AccountsController : Controller
    {
        private readonly db_maketContext _context;

        public AccountsController(db_maketContext context)
        {
            _context = context;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ValidatePhone(string Phone)
        {
            try
            {
                var khachhang = _context.Customers.AsNoTracking().SingleOrDefault(x => x.Phone.ToLower() == Phone.ToLower());
                if (khachhang != null)
                    return Json(data: "Ső điện thoại: " + Phone + "Đã được sử dụng");
                return Json(data: true);

            }
            catch
            {
                return Json(data: true);

            }
        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ValidateEmail(string Email)
        {
            try
            {
                var khachhang = _context.Customers.AsNoTracking().SingleOrDefault(x => x.Phone.ToLower() == Email.ToLower());
                if (khachhang != null)
                    return Json(data: " Email " + Email + "Đã được sử dụng");
                return Json(data: true);

            }
            catch
            {
                return Json(data: true);

            }
        }

        [Route("/tai-khoan.html", Name = "Dashboard")]
        public IActionResult Dashboard()
        {
            var taikhoanID = HttpContext.Session.GetString("CustomerId");
            if (taikhoanID != null)
            {
                var khachhang = _context.Customers.AsNoTracking().SingleOrDefault(x => x.CustomerId == Convert.ToInt32(taikhoanID));
                if(khachhang!=null)
                {
                    return View(khachhang);

                }
            }
            return RedirectToAction("Login");
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("/dang-ky.html", Name = "DangKy")]
        public IActionResult DangkyTaiKhoan()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("/dang-ky.html", Name = "DangKy")]
        public async Task<IActionResult> DangkyTaiKhoan(RegisterVM taikhoan)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string salt = Utilities.GetRandomKey();
                    Customer khachhang = new Customer
                    {
                        FullName = taikhoan.FullName,
                        Phone = taikhoan.Phone.Trim().ToLower(),
                        Email = taikhoan.Email.Trim().ToLower(),
                        Password = (taikhoan.Password + salt.Trim()).ToMD5(),
                        Active = true,
                        Salt = salt,
                        CreateDate = DateTime.Now
                    };
                    try
                    {
                        _context.Add(khachhang);
                        await _context.SaveChangesAsync();
                        // Luu Session MaKh
                        HttpContext.Session.SetString("CustomerId", khachhang.CustomerId.ToString());
                        var taikhoanID = HttpContext.Session.GetString("CustomerId");
                        // Identity
                        var claims = new List<Claim>
                        {
                        new Claim(ClaimTypes.Name, khachhang.FullName),
                        new Claim("CustomerId", khachhang.CustomerId.ToString())
                        };

                        ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "login");
                        ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                        await HttpContext.SignInAsync(claimsPrincipal);
                        return RedirectToAction("Dashboard", "Accounts");

                    }
                    catch(Exception ex)
                    {
                        return RedirectToAction("DangKyTaiKhoan", "Accounts");

                    }

                }

                else
                {
                    return View(taikhoan);
                }
            }
            catch
            {
                return View(taikhoan);

            }
        }

        
        [AllowAnonymous]
        [Route("dang-nhap.html", Name = "DangNhap")]
        public IActionResult Login(string returnUrl = null)
        {
            var taikhoanID = HttpContext.Session.GetString("CustomerId");
            if (taikhoanID != null)
            {
                return RedirectToAction("Dashboard", "Accounts");

            }
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("dang-nhap.html", Name = "DangNhap")]
        public async Task<IActionResult> Login(LoginViewModel customer, string returnUrl = null)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    bool isEmail = Utilities.IsValidEmail(customer.UserName);
                    if (!isEmail) return View(customer);
                    var khachhang = _context.Customers.AsNoTracking().SingleOrDefault(x => x.Email.Trim() == customer.UserName);
                    if (khachhang == null) return RedirectToAction("DangkyTaiKhoan");
                    string pass = (customer.Password + khachhang.Salt.Trim()).ToMD5();
                    if (khachhang.Password != pass)
                    {
                        return View(customer);
                    }
                    if (khachhang.Active == false) return RedirectToAction("ThongBao", "Accounts");



                    HttpContext.Session.SetString("CustomerId", khachhang.CustomerId.ToString());
                    var taikhoanID = HttpContext.Session.GetString("CustomerId");
                    //Identity
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, khachhang.FullName),
                        new Claim("CustomerId", khachhang.CustomerId.ToString())
                    };

                    ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "login");
                    ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                    await HttpContext.SignInAsync(claimsPrincipal);
                    return RedirectToAction("Dashboard", "Accounts");
                }

            }
            catch
            {
                return RedirectToAction("DangkyTaikhoan", "Accounts");
            }
            return View(customer);
        }
        [HttpGet]
        [Route("/dang-xuat.html",Name="Dangxuat")]
        public IActionResult Logout()
        {
           
            HttpContext.SignOutAsync();
            HttpContext.Session.Remove("CustomerId");
            HttpContext.Session.Remove("GioHang");
            return RedirectToAction("Index", "Home");
        }

    }
}
