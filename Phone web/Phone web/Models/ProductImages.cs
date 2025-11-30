using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Phone_web.Models
{
    public class ProductImages : Controller
    {
        // GET: ProductImages
        public int ImageID { get; set; }       // Khóa chính
        public int ProID { get; set; }         // FK tới Product
        public int? ColorID { get; set; }      // FK tới Color (tùy chọn)
        public string ImagePath { get; set; }  // Đường dẫn ảnh

        // Navigation properties
        public virtual Product Product { get; set; }
        public virtual Color Color { get; set; }
    }
}