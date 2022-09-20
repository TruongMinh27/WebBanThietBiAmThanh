using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebBanThietBiAmThanh.Models;

namespace WebBanThietBiAmThanh.ModelViews
{
    public class HomeViewVM
    {
        public List<TinTuc> TinTucs { get; set; }
        public List<ProductHomeVM> Products { get; set; }
    
    }
}
