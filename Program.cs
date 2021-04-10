using System;
using System.Linq;

namespace FinalPractice
{
    class Program
    {
        static void Main(string[] args)
        {

            using (ShopContext shopContext = new ShopContext())
            {
                ////Adding Customers
                //shopContext.customers.Add(new Customer() { CustomerID = 1, Name = "Nacho Baldi", });
                //shopContext.customers.Add(new Customer() { CustomerID = 2, Name = "Uri Baldi" });
                //shopContext.customers.Add(new Customer() { CustomerID = 3, Name = "Jonny Bravo" });
                //shopContext.customers.Add(new Customer() { CustomerID = 4, Name = "Google Mogle" });

                //shopContext.SaveChanges();

                ////Adding Customers Details
                //shopContext.customerDetails.Add(new CustomerDetail() { CustomerID = 1, Email = "nachoxbaldi@gmail.com", PhoneNum = "0542226303" });
                //shopContext.customerDetails.Add(new CustomerDetail() { CustomerID = 2, Email = "uri@gmail.com", PhoneNum = "0508784589" });
                //shopContext.customerDetails.Add(new CustomerDetail() { CustomerID = 3, Email = "JB@gmail.com", PhoneNum = "0535852135" });
                //shopContext.customerDetails.Add(new CustomerDetail() { CustomerID = 4, Email = "Goomogle@gmail.com", PhoneNum = "0525564781" });
                //shopContext.SaveChanges();

                ////Adding Orders
                //shopContext.orders.Add(new Order() { CustomerID = 1, Created = DateTime.Now });
                //shopContext.orders.Add(new Order() { CustomerID = 2, Created = DateTime.Now });
                //shopContext.orders.Add(new Order() { CustomerID = 3, Created = DateTime.Now });
                //shopContext.orders.Add(new Order() { CustomerID = 1, Created = DateTime.Now });
                //shopContext.SaveChanges();

                ////Adding Products
                //shopContext.products.Add(new Product() { ProductName = "Sugar", Price = 2.99 });
                //shopContext.products.Add(new Product() { ProductName = "Coca Cola", Price = 5.60 });
                //shopContext.products.Add(new Product() { ProductName = "Gum", Price = 10 });
                //shopContext.products.Add(new Product() { ProductName = "Bread", Price = 6.50 });
                //shopContext.products.Add(new Product() { ProductName = "Tomato", Price = 3.90 });
                //shopContext.SaveChanges();

                ////Adding Order Products
                //shopContext.order_Products.Add(new Order_Product() { OrderID = 1, ProductID = 2, Quantity = 4 });
                //shopContext.order_Products.Add(new Order_Product() { OrderID = 1, ProductID = 4, Quantity = 1 });
                //shopContext.order_Products.Add(new Order_Product() { OrderID = 1, ProductID = 5, Quantity = 2 });

                //shopContext.order_Products.Add(new Order_Product() { OrderID = 2, ProductID = 1, Quantity = 2 });
                //shopContext.order_Products.Add(new Order_Product() { OrderID = 2, ProductID = 4, Quantity = 1 });

                //shopContext.order_Products.Add(new Order_Product() { OrderID = 3, ProductID = 3, Quantity = 3 });
                //shopContext.order_Products.Add(new Order_Product() { OrderID = 3, ProductID = 2, Quantity = 1 });

                //shopContext.order_Products.Add(new Order_Product() { OrderID = 4, ProductID = 2, Quantity = 1 });
                //shopContext.SaveChanges();

                //if (shopContext.customers != null)
                //{

                //    var cus = from x in shopContext.customers where x != null select x;
                //    foreach (var item in cus)
                //    {
                //        System.Console.WriteLine(item);
                //    }
                //}
                //else
                //{
                //    System.Console.WriteLine("no entry");
                //}

                Console.WriteLine("\n3. Select all customers with orders that have products.");

                var allCustomersThatHaveProducts = from x in shopContext.customers
                                                   join o in shopContext.orders on x.CustomerID equals o.CustomerID

                                                   select x;
                foreach (var item in allCustomersThatHaveProducts)
                {
                    System.Console.WriteLine(item);
                }
                Console.WriteLine("---------------------------------------------------------------------------------------------");
                Console.WriteLine("\n4. Select all customers orders summery values(Customers.Name , OrderID, Value) fields");
                var valuePerOrder = (from c in shopContext.customers
                                   join o in shopContext.orders on c.CustomerID equals o.CustomerID
                                   join op in shopContext.order_Products on o.OrderID equals op.OrderID
                                   join p in shopContext.products on op.ProductID equals p.ProductID
                                   group new { p.Price, op.Quantity } by new { op.OrderID, c.CustomerID, c.Name } into gg
                                   select new { gg.Key, value = gg.Sum(x => x.Price * x.Quantity) }
                                  ).ToList();
                valuePerOrder.ForEach(x => Console.WriteLine($"Name: {x.Key.Name} \nOrderID: {x.Key.OrderID} --> Total Price: {x.value.ToString("C")}\n"));

                Console.WriteLine("---------------------------------------------------------------------------------------------");
                Console.WriteLine("\n5. Select all customers orders that do not have products");

                //Select all customers orders that do not have products
                var customersThatNoHaveOrders = (from c in shopContext.customers
                                                 join o in shopContext.orders on c.CustomerID equals o.CustomerID into cus
                                                 from cutom in cus.DefaultIfEmpty()
                                                 where String.IsNullOrEmpty(cutom.OrderID.ToString())
                                                 select new { c.CustomerID, c.Name }).ToList();

                customersThatNoHaveOrders.ForEach(x => Console.WriteLine(x));



                Console.WriteLine("---------------------------------------------------------------------------------------------");
                Console.WriteLine("\n6. Select the product that been ordered that most");

                //Option 1: take the max
                Console.WriteLine("\nOption 1 ");
                var productMostOrdered = (from p in shopContext.products
                                           join op in shopContext.order_Products on p.ProductID equals op.ProductID
                                           join o in shopContext.orders on op.OrderID equals o.OrderID
                                           group new { op, p } by new { p.ProductID, p.ProductName } into gg
                                           where gg.Sum(x => x.op.Quantity).Equals((from op1 in shopContext.order_Products
                                                                                    group op1 by op1.ProductID into gg1
                                                                                    orderby gg1.Sum(x => x.Quantity) descending
                                                                                    select gg1.Sum(x => x.Quantity)).First())
                                           select new { gg.Key.ProductName, TimesOrdered = gg.Sum(x => x.op.Quantity) }).ToList();
                
                
                productMostOrdered.ForEach(x => Console.WriteLine(x));



                //option 2 : take the top 1
                Console.WriteLine("\nOption 2 ");

                var productMostOrdered1 = (from p in shopContext.products
                                         join op in shopContext.order_Products on p.ProductID equals op.ProductID
                                         join o in shopContext.orders on op.OrderID equals o.OrderID
                                         group new { op, p } by new { p.ProductID, p.ProductName } into gg
                                         orderby gg.Sum(x => x.op.Quantity) descending
                                         select new { gg.Key.ProductName, TimesOrdered = gg.Sum(x => x.op.Quantity) }).First();
               
                Console.WriteLine(productMostOrdered1);
                

            }

        }
    }
}
