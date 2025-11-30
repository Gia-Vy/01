using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace Phone_web.Models
{
    public class customerHome
    {
        public int ProID { get; set; }
        public int CatID { get; set; }
        public string ProName { get; set; }
        public string ProImage { get; set; }
        public DateTime CreateDate { get; set; }
        public string NameDescription { get; set; }
        public decimal Price { get; set; }
        public Category Category { get; set; }
        public bool IsNew { get; set; }
        public virtual ICollection<ProductDetail> ProductDetails { get; set; }
        public virtual ICollection<ProductImage> ProductImages { get; set; }

    }
    public partial class Product
    {
        // Dùng để nhận file upload từ form, không map vào DB
        public HttpPostedFileBase ImagePath { get; set; }
    }
}