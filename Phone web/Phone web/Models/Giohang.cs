using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace Phone_web.Models
{
    public class Giohang
    {
    public int idPro { get; set; }          // ProDeID
    public string namePro { get; set; }     // Tên sản phẩm
    public string ColorName { get; set; }   // Màu sắc
    public string DungLuongName { get; set; } // Dung lượng
    public int quantity { get; set; }
    public decimal price { get; set; }
    public decimal total => price * quantity;
        public string ProImage { get; set; }
    }
}