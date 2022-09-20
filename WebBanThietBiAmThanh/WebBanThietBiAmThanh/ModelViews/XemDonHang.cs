using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebBanThietBiAmThanh.Models;

namespace WebBanThietBiAmThanh.ModelViews
{
    public class XemDonHang
    {
        public Order DonHang { get; set; }
        public List<OrderDetail> ChiTietDonHang { get; set; }
    }
}
