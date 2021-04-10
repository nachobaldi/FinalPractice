using Microsoft.EntityFrameworkCore;

namespace FinalPractice
{
    public class ShopContext : DbContext
    {
        public DbSet<Customer> customers { get; set; }
        public DbSet<CustomerDetail> customerDetails { get; set; }
        public DbSet<Order> orders { get; set; }
        public DbSet<Product> products { get; set; }
        public DbSet<Order_Product> order_Products { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer("Data Source =DESKTOP-1SEMVTT\\SQLEXPRESS; Initial Catalog = Shop; Integrated Security = True");
        }
        //Properties Changes
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>()
                .HasKey(x => x.CustomerID);
            modelBuilder.Entity<Customer>()
                .Property(x => x.Name).HasMaxLength(16);
            modelBuilder.Entity<Customer>()
                .Property(x => x.CustomerID).ValueGeneratedNever();

            modelBuilder.Entity<CustomerDetail>()
                .HasIndex(x => x.Email).IsUnique();

            modelBuilder.Entity<CustomerDetail>()
                .HasIndex(x => x.PhoneNum).IsUnique();
            modelBuilder.Entity<CustomerDetail>()
                .HasKey(x => x.CustomerID);

            modelBuilder.Entity<CustomerDetail>()
                .Property(x => x.PhoneNum).IsRequired();


            modelBuilder.Entity<CustomerDetail>()
                .HasOne(x => x.Customer)
                .WithOne(x => x.CustomerDetail);

            modelBuilder.Entity<Order>()
                .HasKey(x => x.OrderID);

            modelBuilder.Entity<Order>()
                .HasOne(x => x.Customer)
                .WithMany(x => x.Order)
                .HasForeignKey(x => x.CustomerID);
            

            modelBuilder.Entity<Product>()
                .HasKey(x => x.ProductID);
            modelBuilder.Entity<Product>()
                .HasIndex(x => x.ProductName).IsUnique();
            modelBuilder.Entity<Product>()
                .Property(x => x.ProductName).HasMaxLength(16);

            modelBuilder.Entity<Order_Product>()
                .HasOne(x => x.Order)
                .WithMany(x => x.Order_Product);



            modelBuilder.Entity<Order_Product>().HasKey(x => new { x.OrderID, x.ProductID });




        }
    }
}
