using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace FinalPractice
{
     public class Customer
    {
        [Key]
        public int? CustomerID { get; set; }
        public string Name { get; set; }
        public CustomerDetail? CustomerDetail { get; set; }

        public List<Order>? Order{ get; set; }
        public override string ToString()
        {
            return $"CustomerID: {CustomerID}, Name: {Name}";
        }

    }
}
