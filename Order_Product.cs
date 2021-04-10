using System;
using System.Collections.Generic;
using System.Text;

namespace FinalPractice
{
    public class Order_Product
    {
        public int OrderID { get; set; }
        public int ProductID { get; set; }
        public int Quantity { get; set; }
        public Order Order { get; set; }
        public Product Product { get; set; }
    }
}
