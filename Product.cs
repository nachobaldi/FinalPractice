using System;
using System.Collections.Generic;
using System.Text;

namespace FinalPractice
{
    public class Product
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public double Price { get; set; }
        public List<Order_Product> order_Product { get; set; }
    }
}
