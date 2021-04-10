using System;
using System.Collections.Generic;
using System.Text;

namespace FinalPractice
{
    public class Order
    {
        public int OrderID { get; set; }
        public DateTime Created { get; set; }
        public int CustomerID { get; set; }
        public Customer Customer { get; set; }
        public List<Order_Product> Order_Product { get; set; }

    }
}
