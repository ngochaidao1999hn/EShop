using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EShop.Data
{
    public class Cart
    {
        public int Pro_id { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public string Image { get; set; }
        public string Name { get; set; }
        public Cart(int pro_id, int quantity, decimal price,string image,string name)
        {
            Pro_id = pro_id;
            Quantity = quantity;
            Price = price;
            Image = image;
            Name = name;
        }
    }
}