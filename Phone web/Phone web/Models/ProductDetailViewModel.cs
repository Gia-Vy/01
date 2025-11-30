using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Phone_web.Models
{
    public class ProductDetailViewModel
    {
        public int ProID { get; set; }
        public string ProName { get; set; } = string.Empty;
        public string ProDescription { get; set; } = string.Empty;
        public string ProImage { get; set; } = string.Empty;

        public List<ProductVersionViewModel> Versions { get; set; } = new List<ProductVersionViewModel>();
        public List<string> Images { get; set; } = new List<string>();
    }

    public class ProductVersionViewModel
    {
        public int ProDeID { get; set; }
        public string ColorName { get; set; }
        public string DungLuongName { get; set; }
        public decimal Price { get; set; }
        public int RemainQuantity { get; set; }
        public int SoldQuantity { get; set; }
    }

    

   


}